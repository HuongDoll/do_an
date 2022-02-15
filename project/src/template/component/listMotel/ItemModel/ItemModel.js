import React, { useState, useEffect } from 'react'
import image from './../../../../styles/Image/image.jpg'
import PropTypes from 'prop-types'

import './ItemModel.scss'

ItemModel.propTypes = {
  onClickItem: PropTypes.func,
  data: PropTypes.object
}

ItemModel.defaultProps = {
  onClickItem: () => {},
  data: {}
}

function ItemModel (props) {
  return (
    <div
      className='motel-login'
      onClick={() => {
        props.onClickItem()
      }}
    >
      <div className='motel-login__left'>
        <div className='time'>{props.data?.modifiedDate}</div>
        <img
          className='image'
          src={
            props.data?.urlThumbnail
              ? `https://localhost:44342/api/public/files/${props.data?.urlThumbnail}`
              : image
          }
        ></img>
      </div>
      <div className='motel-login__right'>
        <div
          className='sub-title'
          dangerouslySetInnerHTML={{
            __html:
              props?.data?.content ||
              `<h3><a href="https://ant.design/components/upload/?fbclid=IwAR0gNEGVlqr0ouY9TPqd0yzEDeIcl39I7k2wY2jo77sggt3f0BvOowC1q9Y#components-upload-demo-picture-style">Chính chủ cho thuê chung cư mini mới xây, nhà xây 6 tầng có thang máy.</a></h3><p>Vị trí: Số 21 ngõ 1 đường Đại Linh, Trung Văn, Nam Từ Liêm, Hà Nội. Ô tô đỗ cửa, Nhà cách đường Lê Văn Lương chỉ chỉ 200m, cách đường Đại Lộ Thăng Long 400m, đi sang KeangNam chỉ 10 phút, đi sang khu đô thị trung Hòa 10 phút,. Xung quanh tòa nhà có rất nhiều quán ăn, quán cafe, và nhiều tiện ích nữa...</p>`
          }}
        ></div>

        {/* <div className='user'>Đăng bởi: Hoang Thi Thu Huong</div> */}
      </div>
    </div>
  )
}

export default ItemModel
