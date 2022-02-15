import React, { useState, useEffect } from 'react';
import BasicColumn from '../chart/BasicColumn/BasicColumn';

import './Revenue.scss';
import BasicDonut from './../chart/BasicDonut/BasicDonut';

function Revenue() {
  return (
    <div className='motel-revenue'>
      <div className='motel-revenue__lable'>Thống kê doanh thu</div>
      <div className='motel-revenue__content'>
        <div className='motel-revenue__content-left'>
          <BasicDonut />
        </div>
        <div className='motel-revenue__content-right'>
          <BasicColumn />
          <p>Biểu đồ doanh thu</p>
        </div>
      </div>
    </div>
  );
}

export default Revenue;
