import React, { useState, useEffect } from 'react'

import './BodyItem.scss'

function BodyItem () {
  return (
    <div className='motel-invoice-body-item row '>
      <div className='row_inf '>
        Địa chỉ: Số 12, ngõ 12, Lê Thanh Nghị, Bách Khoa, Hai Bà Trưng, Hà Nội
      </div>
      <div className='row_host '>
        <div>Hoang Thi Thu Huong</div>
        <div>0973843806</div>
      </div>

      <div className='row_cost '>5.000.000</div>
      <div className='row_status '>Chưa thanh toán</div>
    </div>
  )
}

export default BodyItem
