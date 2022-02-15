import React, { useState, useEffect } from 'react'
import image from './../../../../styles/Image/image.jpg'
import { EditOutlined, DeleteTwoTone } from '@ant-design/icons'
import PropTypes from 'prop-types'

import { Button } from 'antd'

import './BodyItem.scss'

BodyItem.propTypes = {
  onClickItem: PropTypes.func,
  data: PropTypes.object
}

BodyItem.defaultProps = {
  onClickItem: () => {},
  data: {}
}

function BodyItem (props) {
  return (
    <div
      className='motel-body-item row '
      onClick={() => {
        props.onClickItem()
      }}
    >
      <div className='row_inf '>
        <img
          className='image'
          src={
            props.data?.urlThumbnail
              ? `https://localhost:44342/api/public/files/${props.data?.urlThumbnail}`
              : image
          }
        ></img>

        <div className='row_inf_content'>
          <div className='address'>{props.data?.address}</div>
          <div className='area'>Diện tích: {props.data?.area} m2</div>
          <div className='cost'>Giá: {props.data?.price}/ tháng</div>
          <div className='user'>Đăng bởi: Hoang Thi Thu Huong</div>
        </div>
      </div>
      <div className='row_status '>Còn trống</div>
      <div className='row_option '>
        <Button icon={<EditOutlined />} type='text'></Button>
        <Button
          icon={<DeleteTwoTone twoToneColor='red' />}
          type='text'
        ></Button>
      </div>
    </div>
  )
}

export default BodyItem
