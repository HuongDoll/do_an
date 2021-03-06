import React, { useState, useEffect } from 'react'
import { Input, InputNumber } from 'antd'
import ItemModel from './ItemModel/ItemModel'
import { Button, Tooltip, Select } from 'antd'
import { SearchOutlined } from '@ant-design/icons'
import axios from 'axios'

import './listMotel.scss'
import PostDetail from '../PostDetail/PostDetail'

function ListMotel () {
  const { Option } = Select

  const [isClickItem, setIsClickItem] = useState(false)
  const [province, setProvince] = useState()
  const [selectProvince, setSelectProvince] = useState()
  const [district, setDistrict] = useState()
  const [selectDistrict, setSelectDistrict] = useState()
  const [ward, setWard] = useState()
  const [selectWard, setSelectWard] = useState()
  const [minPrice, setMinPrice] = useState(null)
  const [maxPrice, setMaxPrice] = useState(null)
  const [minArea, setMinArea] = useState(null)
  const [maxArea, setMaxArea] = useState(null)
  const [data, setData] = useState()

  useEffect(() => {
    getData()
    getProvince()
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
  function removeVietnameseTones (str) {
    str = str.replace(/??|??|???|???|??|??|???|???|???|???|???|??|???|???|???|???|???/g, 'a')
    str = str.replace(/??|??|???|???|???|??|???|???|???|???|???/g, 'e')
    str = str.replace(/??|??|???|???|??/g, 'i')
    str = str.replace(/??|??|???|???|??|??|???|???|???|???|???|??|???|???|???|???|???/g, 'o')
    str = str.replace(/??|??|???|???|??|??|???|???|???|???|???/g, 'u')
    str = str.replace(/???|??|???|???|???/g, 'y')
    str = str.replace(/??/g, 'd')
    str = str.replace(/??|??|???|???|??|??|???|???|???|???|???|??|???|???|???|???|???/g, 'A')
    str = str.replace(/??|??|???|???|???|??|???|???|???|???|???/g, 'E')
    str = str.replace(/??|??|???|???|??/g, 'I')
    str = str.replace(/??|??|???|???|??|??|???|???|???|???|???|??|???|???|???|???|???/g, 'O')
    str = str.replace(/??|??|???|???|??|??|???|???|???|???|???/g, 'U')
    str = str.replace(/???|??|???|???|???/g, 'Y')
    str = str.replace(/??/g, 'D')
    // Some system encode vietnamese combining accent as individual utf-8 characters
    // M???t v??i b??? encode coi c??c d???u m??, d???u ch??? nh?? m???t k?? t??? ri??ng bi???t n??n th??m hai d??ng n??y
    str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, '') // ?? ?? ?? ?? ??  huy???n, s???c, ng??, h???i, n???ng
    str = str.replace(/\u02C6|\u0306|\u031B/g, '') // ?? ?? ??  ??, ??, ??, ??, ??
    // Remove extra spaces
    // B??? c??c kho???ng tr???ng li???n nhau
    str = str.replace(/ + /g, ' ')
    str = str.trim()
    // Remove punctuations
    // B??? d???u c??u, k?? t??? ?????c bi???t
    str = str.replace(
      /!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g,
      ' '
    )
    return str
  }

  /**
   * l???y danh s??ch ph??ng
   */
  const getData = function () {
    var token = localStorage.getItem('access')
    var hostId = localStorage.getItem('userID')

    var filter = [
      {
        key: 'Status',
        Value: 4,
        operator: 1
      }
    ]

    if (selectProvince?.code != undefined)
      filter.push({
        key: 'Province',
        Value: removeVietnameseTones(
          selectProvince?.name
            .replace('Th??nh ph???', '')
            .replace('T???nh', '')
            .trim()
        ),
        operator: 1
      })

    if (selectDistrict?.code != undefined)
      filter.push({
        key: 'District',
        Value: removeVietnameseTones(
          selectDistrict?.name
            .replace('Th??nh ph???', '')
            .replace('Qu???n', '')
            .replace('Huy???n', '')
            .trim()
        ),
        operator: 1
      })

    if (selectWard?.code != undefined)
      filter.push({
        key: 'Ward',
        Value: removeVietnameseTones(
          selectWard?.name
            .replace('Th??? tr???n', '')
            .replace('Ph?????ng', '')
            .replace('X??', '')
            .trim()
        ),
        operator: 1
      })

    if (minArea != null)
      filter.push({
        key: 'Area',
        Value: minArea.trim(),
        operator: 5
      })

    if (maxArea != null)
      filter.push({
        key: 'Area',
        Value: maxArea.trim(),
        operator: 6
      })

    if (minPrice != null)
      filter.push({
        key: 'Price',
        Value: minPrice.trim(),
        operator: 5
      })

    if (maxPrice != null)
      filter.push({
        key: 'Price',
        Value: maxPrice.trim(),
        operator: 6
      })

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
            'HostID',
            'Content'
          ],
          filters: [filter],
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
        setData(res?.data?.data)
      })
  }

  return !isClickItem ? (
    <div className='motel-list-motel'>
      <div className='motel-list-motel__list'>
        <div className='lable'> Danh s??ch ph??ng</div>
        {data?.map((dataItem, key) => {
          return (
            <ItemModel
              onClickItem={() => {
                setIsClickItem(true)
              }}
              data={dataItem}
            ></ItemModel>
          )
        })}
      </div>
      <div className='motel-list-motel__filter'>
        <div className='motel-list-motel__filter_search'>
          <div className='title'>T??m ki???m</div>
          <div className='group-filter'>
            <span>T???nh/Th??nh ph???</span>
            <Select
              showSearch
              style={{ width: '100%', height: '32px' }}
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
          </div>
          <div className='group-filter'>
            <span>Qu???n/Huy???n</span>
            <Select
              showSearch
              style={{ width: '100%', height: '32px' }}
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
          </div>
          <div className='group-filter'>
            <span>Ph?????ng/X??</span>
            <Select
              showSearch
              style={{ width: '100%', height: '32px' }}
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
          </div>
          {/* <div className='group-filter'>
            <span>???????ng/Ph???</span>
            <Input placeholder='???????ng/Ph???' />
          </div> */}
          <div className='group-filter'>
            <span>Di???n t??ch (m2)</span>
            <div className='group-input'>
              <InputNumber
                min={0}
                // defaultValue={0}
                onChange={value => {
                  setMinArea(value)
                }}
              />
              <span>-</span>
              <InputNumber
                min={1}
                // defaultValue={50}
                onChange={value => {
                  setMaxArea(value)
                }}
              />
            </div>
          </div>

          <div className='group-filter'>
            <span>Gi??(VND)</span>
            <div className='group-input'>
              <InputNumber
                min={0}
                // defaultValue={0}
                onChange={value => {
                  setMinPrice(value)
                }}
              />
              <span>-</span>
              <InputNumber
                min={1}
                // defaultValue={10000000}
                onChange={value => {
                  setMaxPrice(value)
                }}
              />
            </div>
          </div>
        </div>
        {/* <div className='motel-list-motel__filter_sort'>
          <div className='title'>S???p x???p</div>
        </div> */}
        <div className='button'>
          <Button
            type='primary'
            icon={<SearchOutlined />}
            onClick={() => {
              getData()
            }}
          >
            T??m ki???m
          </Button>
        </div>
      </div>
    </div>
  ) : (
    <PostDetail
      onClickBack={() => {
        setIsClickItem(false)
      }}
    ></PostDetail>
  )
}

export default ListMotel
