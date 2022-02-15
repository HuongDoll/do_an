import React, { useState, useEffect } from 'react'
import { Select } from 'antd'

import './myMotel.scss'
import BodyItem from './BodyItem/BodyItem'
import RoomDetail from '../RoomDetail/RoomDetail'

function MyMotel () {
  const { Option } = Select
  const [isClickItem, setIsClickItem] = useState(false)

  return !isClickItem ? (
    <div className='motel-mymotel'>
      <div className='motel-mymotel__lable'>Phòng của tôi</div>
      <div className='motel-mymotel__content'>
        <div className='motel-mymotel__content_filter'>
          <div className='motel-mymotel__content_filter_lable'>
            Trạng thái phòng
          </div>
          <Select
            defaultValue='0'
            style={{ width: 220 }}
            //   onChange={handleChange}
            className='motel-mymotel__content_filter_select'
          >
            <Option value='0'>Tất cả</Option>
            <Option value='1'>Đang thuê</Option>
            <Option value='2'>Đã thuê</Option>
            <Option value='3'>Đang chờ xử lý yêu cầu thuê</Option>
          </Select>
        </div>
        <div className='motel-mymotel__content_table'>
          <div className='row header'>
            <div className='row_inf header'>Thông tin</div>
            <div className='row_status header'>Trạng thái</div>
            <div className='row_option header'>Tùy chọn</div>
          </div>
          <BodyItem
            onClickItem={() => {
              setIsClickItem(true)
            }}
          ></BodyItem>
          <BodyItem
            onClickItem={() => {
              setIsClickItem(true)
            }}
          ></BodyItem>
          <BodyItem
            onClickItem={() => {
              setIsClickItem(true)
            }}
          ></BodyItem>
          <BodyItem
            onClickItem={() => {
              setIsClickItem(true)
            }}
          ></BodyItem>
          <BodyItem
            onClickItem={() => {
              setIsClickItem(true)
            }}
          ></BodyItem>
          <BodyItem
            onClickItem={() => {
              setIsClickItem(true)
            }}
          ></BodyItem>
        </div>
      </div>
    </div>
  ) : (
    <RoomDetail
      onClickBack={() => {
        setIsClickItem(false)
      }}
    ></RoomDetail>
  )
}

export default MyMotel
