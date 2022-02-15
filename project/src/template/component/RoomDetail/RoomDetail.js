import React, { useState, useEffect } from 'react'
import { Button, Tooltip, Image } from 'antd'
import {
  PlusOutlined,
  LeftOutlined,
  SaveOutlined,
  SendOutlined
} from '@ant-design/icons'
import { Row, Col } from 'antd'
import { List, Avatar } from 'antd'
import axios from 'axios'

import PropTypes from 'prop-types'

import './RoomDetail.scss'

RoomDetail.propTypes = {
  onClickBack: PropTypes.func,
  data: PropTypes.object
}

RoomDetail.defaultProps = {
  onClickBack: () => {},
  data: {}
}
const data = [
  {
    title: 'Địa chỉ',
    content: '',
    avt:
      'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTzpQ5YenVypFBtbZCXd2pW5euFnknS_sBc_qh-0b-02HPC6XX2rOS5c0k_0MkCdY_LGNs&usqp=CAU'
  },
  {
    title: 'Diện tích',
    content: ' m2',
    avt:
      'https://cdn3.iconfinder.com/data/icons/real-estate-flat-icons-vol-2/256/68-512.png'
  },
  {
    title: 'Giá',
    content: '  VND',
    avt:
      'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSFO-CSGZKlqR5nGD1wTguWPd0r0roaRt5Pr4ONom-UBJlvvJdq1a9VQC_JR-H_f_On8uY&usqp=CAU'
  },
  {
    title: 'Chủ trọ',
    content: ' hoang thi thu huong',
    avt: 'https://blog.cpanel.com/wp-content/uploads/2019/08/user-01.png'
  }
]

function RoomDetail (props) {
  const [dataRoom, setDataRoom] = useState([
    {
      title: 'Địa chỉ',
      content: '',
      avt:
        'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTzpQ5YenVypFBtbZCXd2pW5euFnknS_sBc_qh-0b-02HPC6XX2rOS5c0k_0MkCdY_LGNs&usqp=CAU'
    },
    {
      title: 'Diện tích',
      content: ' m2',
      avt:
        'https://cdn3.iconfinder.com/data/icons/real-estate-flat-icons-vol-2/256/68-512.png'
    },
    {
      title: 'Giá',
      content: '  VND',
      avt:
        'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSFO-CSGZKlqR5nGD1wTguWPd0r0roaRt5Pr4ONom-UBJlvvJdq1a9VQC_JR-H_f_On8uY&usqp=CAU'
    },
    {
      title: 'Chủ trọ',
      content: ' hoang thi thu huong',
      avt: 'https://blog.cpanel.com/wp-content/uploads/2019/08/user-01.png'
    }
  ])

  const [listLink, setListLink] = useState([])

  useEffect(() => {
    setDataRoom([
      {
        title: 'Địa chỉ',
        content: props?.data?.address,
        avt:
          'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTzpQ5YenVypFBtbZCXd2pW5euFnknS_sBc_qh-0b-02HPC6XX2rOS5c0k_0MkCdY_LGNs&usqp=CAU'
      },
      {
        title: 'Diện tích',
        content: props?.data?.area + ' m2',
        avt:
          'https://cdn3.iconfinder.com/data/icons/real-estate-flat-icons-vol-2/256/68-512.png'
      },
      {
        title: 'Giá',
        content: props?.data?.price + ' VND',
        avt:
          'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSFO-CSGZKlqR5nGD1wTguWPd0r0roaRt5Pr4ONom-UBJlvvJdq1a9VQC_JR-H_f_On8uY&usqp=CAU'
      },
      {
        title: 'Chủ trọ',
        content: ' hoang thi thu huong',
        avt: 'https://blog.cpanel.com/wp-content/uploads/2019/08/user-01.png'
      }
    ])
  }, [props.data])

  useEffect(() => {
    getImage()
  }, [props.data?.roomID])

  const getImage = () => {
    var token = localStorage.getItem('access')

    axios
      .post(
        `https://localhost:44342/api/files/filter`,
        {
          selectedFields: ['UrlRaw'],
          filters: [
            [
              {
                key: 'RoomID',
                valueArray: [],
                Value: props.data?.roomID,
                operator: 0
              }
            ]
          ],
          orders: ['-ModifiedBy']
        },
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      )
      .then(res => {
        var links = []
        for (let link of res?.data?.data) {
          links.push(`https://localhost:44342/api/public/files/${link.urlRaw}`)
        }
        setListLink(links)
      })
  }

  return (
    <div className='room-detail'>
      <div className='room-detail__lable'>
        <div>
          <Button
            icon={<LeftOutlined />}
            size='large'
            type='text'
            onClick={() => props.onClickBack()}
          />
          Chi tiết phòng trọ
        </div>
        <div>
          <Button
            // icon={<SendOutlined />}
            size='large'
            type='primary'
            onClick={() => {
              console.log(listLink)
            }}
          >
            Trả phòng
          </Button>
        </div>
      </div>
      {/* <div className='room - detail__status'>
        <h2>Đang thuê</h2>
      </div> */}
      <Row className='room-detail__content'>
        <Col
          className='room-detail__content_image'
          xs={24}
          sm={24}
          md={24}
          lg={16}
          xl={16}
        >
          {/* <Image.PreviewGroup>
            {listLink[0] && (
              <div className='room-detail__content_image_row'>
                <Image width={'100%'} src={listLink[0]} />
              </div>
            )}
            <div className='room-detail__content_image_row'>
              {listLink[1] && <Image width={'25%'} src={listLink[1]} />}
              {listLink[2] && <Image width={'25%'} src={listLink[2]} />}
              {listLink[3] && <Image width={'25%'} src={listLink[3]} />}
              {listLink[4] && <Image width={'25%'} src={listLink[4]} />}
            </div>
          </Image.PreviewGroup> */}
          <Image.PreviewGroup>
            <Row>
              <Col
                className='room-detail__content_image_row'
                xs={24}
                sm={24}
                md={24}
                lg={24}
                xl={18}
              >
                <Image width={'100%'} src={listLink[0]} />
              </Col>
              <Col
                className='room-detail__content_image_row'
                xs={24}
                sm={24}
                md={24}
                lg={24}
                xl={6}
              >
                <Row>
                  {listLink.map((data, key) => {
                    return key == 0 ? (
                      <></>
                    ) : (
                      <Col
                        xs={6}
                        sm={6}
                        md={6}
                        lg={6}
                        xl={24}
                        className='mini-image'
                      >
                        <Image
                          width={'100%'}
                          src={data}
                          style={{ padding: '2px' }}
                        />
                      </Col>
                    )
                  })}
                </Row>
              </Col>
            </Row>
          </Image.PreviewGroup>
        </Col>
        <Col
          className='room-detail__content_room'
          xs={24}
          sm={24}
          md={24}
          lg={8}
          xl={8}
        >
          <List
            itemLayout='horizontal'
            dataSource={dataRoom}
            size='large'
            renderItem={item => (
              <List.Item>
                <List.Item.Meta
                  avatar={<Avatar src={item.avt} size={'large'} />}
                  title={<h3>{item.title}</h3>}
                  description={item.content}
                />
              </List.Item>
            )}
          />
        </Col>
      </Row>
    </div>
  )
}

export default RoomDetail
