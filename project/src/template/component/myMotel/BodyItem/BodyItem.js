import React, { useState, useEffect } from 'react'
import image from './../../../../styles/Image/image.jpg'
import PropTypes from 'prop-types'

import { Button } from 'antd'

import './BodyItem.scss'

BodyItem.propTypes = {
  onClickItem: PropTypes.func
}

BodyItem.defaultProps = {
  onClickItem: () => {}
}

function BodyItem (props) {
  return (
    <div
      className='motel-my-motel-body-item row '
      onClick={() => {
        props.onClickItem()
      }}
    >
      <div className='row_inf '>
        <img className='image' src={image}></img>

        <div className='row_inf_content'>
          <div className='address'>
            Địa chỉ: Số 12, ngõ 12, Lê Thanh Nghị, Bách Khoa, Hai Bà Trưng, Hà
            Nội
          </div>
          <div className='area'>Diện tích: 25m2</div>
          <div className='cost'>Giá: 2.5000.000 / tháng</div>
          <div className='user'>Đăng bởi: Hoang Thi Thu Huong</div>
        </div>
      </div>
      <div className='row_status '>Đang thuê</div>
      <div className='row_option '>
        <Button>Trả phòng</Button>
      </div>
    </div>
  )
}

export default BodyItem
