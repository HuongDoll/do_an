import React, { useState, useEffect } from 'react'

import { Button } from 'antd'
import { CheckOutlined, CloseOutlined } from '@ant-design/icons'

import './BodyItem.scss'

function BodyItem () {
  return (
    <div className='motel-body-item row '>
      <div className='row_room '>
        Phòng trọ Phòng trọ Phòng trọ Phòng trọ Phòng trọ
      </div>
      <div className='row_customer '>
        <div>Hoang Thi Thu Huong </div>
        <div>0973843806 </div>
      </div>

      <div className='row_time '>2/8/2022 16:45</div>
      <div className='row_action '>
        <Button
          type='primary'
          icon={<CheckOutlined />}
          style={{ marginBottom: '12px', width: '130px' }}
        >
          Chấp nhận
        </Button>
        <br></br>
        <Button
          type='primary'
          icon={<CloseOutlined />}
          style={{ width: '130px' }}
          danger
        >
          Từ chối
        </Button>
      </div>
    </div>
  )
}

export default BodyItem
