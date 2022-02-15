import React, { useState, useEffect } from 'react'
import { Button, Tooltip, Image } from 'antd'
import {
  PlusOutlined,
  LeftOutlined,
  SaveOutlined,
  SendOutlined
} from '@ant-design/icons'
import { Row, Col } from 'antd'

import PropTypes from 'prop-types'

import './PostDetail.scss'

PostDetail.propTypes = {
  onClickBack: PropTypes.func
}

PostDetail.defaultProps = {
  onClickBack: () => {}
}

function PostDetail (props) {
  return (
    <div className='post-detail'>
      <div className='post-detail__lable'>
        <div>
          <Button
            icon={<LeftOutlined />}
            size='large'
            type='text'
            onClick={() => props.onClickBack()}
          />
          Chi tiết bài đăng
        </div>
        <div>
          <Button
            icon={<SendOutlined />}
            size='large'
            type='primary'
            onClick={() => {}}
          >
            Thuê phòng
          </Button>
        </div>
      </div>
      <Row className='post-detail__content'>
        <Col
          className='post-detail__content_image'
          xs={24}
          sm={24}
          md={24}
          lg={12}
          xl={12}
        >
          <Image.PreviewGroup>
            <div className='post-detail__content_image_row'>
              <Image
                width={'100%'}
                src='https://gw.alipayobjects.com/zos/rmsportal/KDpgvguMpGfqaHPjicRK.svg'
              />
            </div>
            <div className='post-detail__content_image_row'>
              <Image
                width={'25%'}
                src='https://gw.alipayobjects.com/zos/antfincdn/aPkFc8Sj7n/method-draw-image.svg'
              />
              <Image
                width={'25%'}
                src='https://zos.alipayobjects.com/rmsportal/jkjgkEfvpUPVyRjUImniVslZfWPnJuuZ.png'
              />
              <Image
                width={'25%'}
                src='https://zos.alipayobjects.com/rmsportal/jkjgkEfvpUPVyRjUImniVslZfWPnJuuZ.png'
              />

              <Image
                width={'25%'}
                src='https://zos.alipayobjects.com/rmsportal/jkjgkEfvpUPVyRjUImniVslZfWPnJuuZ.png'
              />
            </div>
          </Image.PreviewGroup>
        </Col>
        <Col
          className='post-detail__content_post'
          xs={24}
          sm={24}
          md={24}
          lg={12}
          xl={12}
        >
          nooij dung baif ddawngnooij dung baif ddawngnooij dung baif
          ddawngnooij dung baif ddawngnooij dung baif ddawngnooij dung baif
          ddawngnooij dung baif ddawngnooij dung baif ddawngnooij dung baif
          ddawngnooij dung baif ddawngnooij dung baif ddawngnooij dung baif
          ddawngnooij dung baif ddawngnooij dung baif ddawngnooij dung baif
          ddawngnooij dung baif ddawngnooij dung baif ddawngnooij dung baif
          ddawngnooij dung baif ddawngnooij dung baif ddawngnooij dung baif
          ddawng
        </Col>
      </Row>
    </div>
  )
}

export default PostDetail
