import React, { useState, useEffect } from 'react'
import { Select } from 'antd'

import './Invoice.scss'
import BodyItem from './BodyItem/BodyItem'

function Invoice () {
  const { Option } = Select

  return (
    <div className='motel-invoice'>
      <div className='motel-invoice__lable'>Hóa đơn của tôi</div>
      <div className='motel-invoice__content'>
        <div className='motel-invoice__content_filter'>
          <div className='motel-invoice__content_filter_lable'>
            Trạng thái hóa đơn
          </div>
          <Select
            defaultValue='0'
            style={{ width: 220 }}
            //   onChange={handleChange}
            className='motel-invoice__content_filter_select'
          >
            <Option value='0'>Tất cả</Option>
            <Option value='1'>Chưa thanh toán</Option>
            <Option value='2'>Đã thanh toán</Option>
          </Select>
        </div>
        <div className='motel-invoice__content_table'>
          <div className='row header'>
            <div className='row_inf header'>Phòng trọ</div>
            <div className='row_host header'>Chủ trọ</div>

            <div className='row_cost header'>Giá tiền</div>
            <div className='row_status header'>Trạng thái</div>
          </div>
          <BodyItem></BodyItem>
          <BodyItem></BodyItem>
          <BodyItem></BodyItem>
          <BodyItem></BodyItem>
          <BodyItem></BodyItem>
          <BodyItem></BodyItem>
        </div>
      </div>
    </div>
  )
}

export default Invoice
