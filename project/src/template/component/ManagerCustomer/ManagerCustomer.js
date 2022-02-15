import React, { useState, useEffect } from 'react'
import BodyItem from './BodyItem/BodyItem'
import { Tabs } from 'antd'
import { Input, Space } from 'antd'
import { DatePicker } from 'antd'

import './ManagerCustomer.scss'

function ManagerCustomer () {
  const { TabPane } = Tabs
  const { Search } = Input
  const { RangePicker } = DatePicker

  function callback (key) {
    console.log(key)
  }

  return (
    <Tabs onChange={callback} type='card'>
      <TabPane tab='Yêu cầu nhận khách trọ' key='1'>
        <div className='motel-customer'>
          <div className='motel-customer__lable'>
            <div className='title'>Yêu cầu nhận khách trọ</div>
            <div className='filter'>
              <Search
                placeholder='Số điện thoại'
                onSearch={() => {}}
                style={{ width: 200 }}
              />
              <RangePicker />
            </div>
          </div>
          <div className='motel-customer__content'>
            <div className='motel-customer__content_table'>
              <div className='row header'>
                <div className='row_room header'>Phòng trọ</div>
                <div className='row_customer header'>Khách </div>
                <div className='row_time header'>Thời gian</div>

                <div className='row_action header'>Thao tác</div>
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
      </TabPane>
      <TabPane tab='Yêu cầu trả phòng' key='2'>
        <div className='motel-customer'>
          <div className='motel-customer__lable'>
            <div className='title'>Yêu cầu trả phòng</div>
            <div className='filter'>
              <Search
                placeholder='Số điện thoại'
                onSearch={() => {}}
                style={{ width: 200 }}
              />
              <RangePicker />
            </div>
          </div>
          <div className='motel-customer__content'>
            <div className='motel-customer__content_table'>
              <div className='row header'>
                <div className='row_room header'>Phòng trọ</div>
                <div className='row_customer header'>Khách </div>
                <div className='row_time header'>Thời gian</div>
                <div className='row_action header'>Thao tác</div>
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
      </TabPane>
    </Tabs>
  )
}

export default ManagerCustomer
