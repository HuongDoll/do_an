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
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, 'a')
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, 'e')
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, 'i')
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, 'o')
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, 'u')
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, 'y')
    str = str.replace(/đ/g, 'd')
    str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, 'A')
    str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, 'E')
    str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, 'I')
    str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, 'O')
    str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, 'U')
    str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, 'Y')
    str = str.replace(/Đ/g, 'D')
    // Some system encode vietnamese combining accent as individual utf-8 characters
    // Một vài bộ encode coi các dấu mũ, dấu chữ như một kí tự riêng biệt nên thêm hai dòng này
    str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, '') // ̀ ́ ̃ ̉ ̣  huyền, sắc, ngã, hỏi, nặng
    str = str.replace(/\u02C6|\u0306|\u031B/g, '') // ˆ ̆ ̛  Â, Ê, Ă, Ơ, Ư
    // Remove extra spaces
    // Bỏ các khoảng trắng liền nhau
    str = str.replace(/ + /g, ' ')
    str = str.trim()
    // Remove punctuations
    // Bỏ dấu câu, kí tự đặc biệt
    str = str.replace(
      /!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g,
      ' '
    )
    return str
  }

  /**
   * lấy danh sách phòng
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
            .replace('Thành phố', '')
            .replace('Tỉnh', '')
            .trim()
        ),
        operator: 1
      })

    if (selectDistrict?.code != undefined)
      filter.push({
        key: 'District',
        Value: removeVietnameseTones(
          selectDistrict?.name
            .replace('Thành phố', '')
            .replace('Quận', '')
            .replace('Huyện', '')
            .trim()
        ),
        operator: 1
      })

    if (selectWard?.code != undefined)
      filter.push({
        key: 'Ward',
        Value: removeVietnameseTones(
          selectWard?.name
            .replace('Thị trấn', '')
            .replace('Phường', '')
            .replace('Xã', '')
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
        <div className='lable'> Danh sách phòng</div>
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
          <div className='title'>Tìm kiếm</div>
          <div className='group-filter'>
            <span>Tỉnh/Thành phố</span>
            <Select
              showSearch
              style={{ width: '100%', height: '32px' }}
              placeholder='Chọn tỉnh/thành phố'
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
            <span>Quận/Huyện</span>
            <Select
              showSearch
              style={{ width: '100%', height: '32px' }}
              placeholder='Chọn quận/huyện'
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
            <span>Phường/Xã</span>
            <Select
              showSearch
              style={{ width: '100%', height: '32px' }}
              placeholder='Chọn phường/xã'
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
            <span>Đường/Phố</span>
            <Input placeholder='Đường/Phố' />
          </div> */}
          <div className='group-filter'>
            <span>Diện tích (m2)</span>
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
            <span>Giá(VND)</span>
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
          <div className='title'>Sắp xếp</div>
        </div> */}
        <div className='button'>
          <Button
            type='primary'
            icon={<SearchOutlined />}
            onClick={() => {
              getData()
            }}
          >
            Tìm kiếm
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
