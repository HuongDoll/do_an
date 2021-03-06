import React, { useState, useEffect } from 'react'
import { Button, Tooltip, Modal, Input, Select } from 'antd'
import { PlusOutlined } from '@ant-design/icons'
import { Upload } from 'antd'
import ImgCrop from 'antd-img-crop'
import axios from 'axios'

import './ManagerRoom.scss'
import BodyItem from './BodyItem/BodyItem'
import RoomDetail from '../RoomDetail/RoomDetail'

function ManagerRoom () {
  const [isCreatRoom, setIsCreatRoom] = useState(false)

  const { Option } = Select
  const [isClickItem, setIsClickItem] = useState(false)
  const [data, setData] = useState({})
  const [dataSelected, setDataSelected] = useState({})
  const [province, setProvince] = useState()
  const [selectProvince, setSelectProvince] = useState()
  const [district, setDistrict] = useState()
  const [selectDistrict, setSelectDistrict] = useState()
  const [ward, setWard] = useState()
  const [selectWard, setSelectWard] = useState()

  const [room, setRoom] = useState({
    province: ' ',
    district: ' ',
    ward: ' ',
    address: '',
    area: '',
    price: 0,
    status: 0,
    content: ''
  })

  function handleChange (value) {
    setRoom({ ...room, status: value })
  }

  const [fileList, setFileList] = useState([
    // {
    //   uid: '-1',
    //   name: 'image.png',
    //   status: 'done',
    //   url:
    //     'https://zos.alipayobjects.com/rmsportal/jkjgkEfvpUPVyRjUImniVslZfWPnJuuZ.png'
    // }
  ])

  const onChange = ({ fileList: newFileList }) => {
    setFileList(newFileList)
    console.log(fileList)
  }

  const onPreview = async file => {
    let src = file.url
    if (!src) {
      src = await new Promise(resolve => {
        const reader = new FileReader()
        reader.readAsDataURL(file.originFileObj)
        reader.onload = () => resolve(reader.result)
      })
    }
    const image = new Image()
    image.src = src
    const imgWindow = window.open(src)
    imgWindow.document.write(image.outerHTML)
  }

  useEffect(() => {
    getData()
  }, [])

  /**
   * lay du lieu tinh
   */
  const getProvince = function () {
    axios.get(`https://provinces.open-api.vn/api/p/`).then(res => {
      console.log(res)
      console.log(res.data)
      setProvince(res?.data)
    })
  }

  /**
   * lay du lieu huyen
   */
  useEffect(() => {
    if (selectProvince?.code != undefined) getDistrict()
  }, [selectProvince])

  const getDistrict = function () {
    axios
      .get(
        `https://provinces.open-api.vn/api/p/` +
          selectProvince?.code +
          `?depth=2`
      )
      .then(res => {
        console.log(res)
        console.log(res.data)
        setDistrict(res?.data?.districts)
      })
  }

  /**
   * lay du lieu huyen
   */
  useEffect(() => {
    if (selectDistrict?.code != undefined) getWard()
  }, [selectDistrict])

  const getWard = function () {
    axios
      .get(
        `https://provinces.open-api.vn/api/d/` +
          selectDistrict?.code +
          `?depth=2`
      )
      .then(res => {
        console.log(res)
        console.log(res.data)
        setWard(res?.data?.wards)
      })
  }

  /**
   * l???y danh s??ch ph??ng
   */
  const getData = function () {
    var token = localStorage.getItem('access')
    var hostId = localStorage.getItem('userID')

    axios
      .post(
        `https://localhost:44342/api/rooms/filter`,
        {
          selectedFields: [
            'Price',
            'Address',
            'UrlThumbnail',
            'RoomID',
            'Province',
            'District',
            'Ward',
            'Area',
            'Status',
            'UserID',
            'HostID'
          ],
          filters: [
            [
              {
                key: 'HostID',
                valueArray: [],
                Value: hostId,
                operator: 0
              }
            ]
          ],
          orders: ['-ModifiedBy']
        },
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      )
      .then(res => {
        console.log(res)
        console.log(res.data.data)
        setData(res?.data.data)
      })
  }

  /**
   * Tao phong
   */
  const createRoom = function () {
    setIsCreatRoom(false)

    // luu anh

    //tao phong
    var token = localStorage.getItem('access')
    var hostId = localStorage.getItem('userID')

    axios
      .post(
        `https://localhost:44342/api/rooms`,
        {
          hostID: hostId.toString(),
          province: room.province
            .replace('Th??nh ph???', '')
            .replace('T???nh', '')
            .trim(),
          district: room.district
            .replace('Th??nh ph???', '')
            .replace('Qu???n', '')
            .replace('Huy???n', '')
            .trim(),
          ward: room.ward
            .replace('Th??? tr???n', '')
            .replace('Ph?????ng', '')
            .replace('X??', '')
            .trim(),
          address: room.address.trim(),
          area: room.area.trim(),
          price: room.price.trim(),
          status: room.status,
          content: room.content
        },
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      )
      .then(res => {
        setIsCreatRoom(false)
        var formdData = new FormData()
        for (let image of fileList) {
          formdData.append('files', image.originFileObj)
        }
        axios
          .post(
            `https://localhost:44342/api/files/uploads?hostID=${hostId.toString()}&roomID=${
              res.data
            }`,
            formdData,
            {
              headers: {
                Authorization: `Bearer ${token}`
              }
            }
          )
          .then(res => {
            console.log(res)
            setFileList([])
            setData()
          })
      })
  }

  return !isClickItem ? (
    <div className='motel-room'>
      <div className='motel-room__lable'>Qu???n l?? ph??ng tr???</div>
      <div className='motel-room__content'>
        <Button
          type='primary'
          icon={<PlusOutlined />}
          size='large'
          onClick={() => {
            setIsCreatRoom(true)
            getProvince()
          }}
        >
          Th??m ph??ng m???i
        </Button>
        <Modal
          title=' Th??m m???i ph??ng tr???'
          style={{ top: 20 }}
          visible={isCreatRoom}
          onOk={() => {
            createRoom()
          }}
          onCancel={() => setIsCreatRoom(false)}
        >
          <p>T???nh/Th??nh ph??? (*)</p>
          <Select
            showSearch
            style={{ width: '100%', marginBottom: '16px', height: '32px' }}
            placeholder='Ch???n t???nh/th??nh ph???'
            optionFilterProp='children'
            filterOption={(input, option) =>
              option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
            }
            filterSort={(optionA, optionB) =>
              optionA.children
                .toLowerCase()
                .localeCompare(optionB.children.toLowerCase())
            }
            onSelect={value => {
              setSelectProvince(province[value])
              console.log(province[value])
              setRoom({ ...room, province: province[value].name })
            }}
          >
            {province?.map((dataItem, key) => {
              return (
                <Option value={key} key={key}>
                  {dataItem.name}
                </Option>
              )
            })}
          </Select>
          <p>Qu???n/Huy???n (*)</p>

          <Select
            showSearch
            style={{ width: '100%', marginBottom: '16px', height: '32px' }}
            placeholder='Ch???n qu???n/huy???n'
            optionFilterProp='children'
            filterOption={(input, option) =>
              option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
            }
            filterSort={(optionA, optionB) =>
              optionA.children
                .toLowerCase()
                .localeCompare(optionB.children.toLowerCase())
            }
            onSelect={value => {
              setSelectDistrict(district[value])
              console.log(district[value])
              setRoom({ ...room, district: district[value].name })
            }}
          >
            {district?.map((dataItem, key) => {
              return (
                <Option value={key} key={key}>
                  {dataItem.name}
                </Option>
              )
            })}
          </Select>

          <p>Ph?????ng/X?? (*)</p>
          <Select
            showSearch
            style={{ width: '100%', marginBottom: '16px', height: '32px' }}
            placeholder='Ch???n ph?????ng/x??'
            optionFilterProp='children'
            filterOption={(input, option) =>
              option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
            }
            filterSort={(optionA, optionB) =>
              optionA.children
                .toLowerCase()
                .localeCompare(optionB.children.toLowerCase())
            }
            onSelect={value => {
              setSelectWard(ward[value])
              console.log(ward[value])
              setRoom({ ...room, ward: ward[value].name })
            }}
          >
            {ward?.map((dataItem, key) => {
              return (
                <Option value={key} key={key}>
                  {dataItem.name}
                </Option>
              )
            })}
          </Select>

          <p>???????ng/Ph??? (*)</p>
          <Input
            placeholder='???????ng / Ph???
 '
            style={{ marginBottom: '16px' }}
            required
            defaultValue={room.address}
            onChange={value => {
              setRoom({ ...room, address: value.target.value })
            }}
          />
          <p>Di???n t??ch (*)</p>
          <Input
            placeholder='Di???n t??ch'
            style={{ marginBottom: '16px' }}
            required
            defaultValue={room.area}
            onChange={value => {
              setRoom({ ...room, area: value.target.value })
            }}
          />
          <p>Gi?? (*)</p>
          <Input
            placeholder='Gi??'
            style={{ marginBottom: '16px' }}
            required
            defaultValue={room.price}
            onChange={value => {
              setRoom({ ...room, price: value.target.value })
            }}
          />
          <p>Tr???ng th??i</p>
          <Select
            defaultValue={room.status}
            style={{ marginBottom: '16px', width: '100%' }}
            onChange={handleChange}
          >
            <Option value={1}>???? cho thu??</Option>
            <Option value={0}>Ch??a cho thu??</Option>
          </Select>
          <p>H??nh ???nh </p>
          <ImgCrop rotate>
            <Upload
              action=''
              listType='picture-card'
              fileList={fileList}
              onChange={onChange}
              onPreview={onPreview}
            >
              {fileList.length < 5 && '+ Upload'}
            </Upload>
          </ImgCrop>
        </Modal>

        <div className='motel-room__content_table'>
          <div className='row header'>
            <div className='row_inf header'>Th??ng tin</div>
            <div className='row_status header'>Tr???ng th??i</div>
            <div className='row_option header'>T??y ch???n</div>
          </div>
          {data?.length > 0 ? (
            data?.map((dataItem, i) => (
              <BodyItem
                data={dataItem}
                key={i}
                onClickItem={() => {
                  setIsClickItem(true)
                  setDataSelected(dataItem)
                  console.log(dataItem)
                }}
              ></BodyItem>
            ))
          ) : (
            <BodyItem
              onClickItem={() => {
                // setIsClickItem(true)
                console.log('dataItem')
              }}
            ></BodyItem>
          )}
        </div>
      </div>
    </div>
  ) : (
    <RoomDetail
      onClickBack={() => {
        setIsClickItem(false)
      }}
      data={dataSelected}
    ></RoomDetail>
  )
}

export default ManagerRoom
