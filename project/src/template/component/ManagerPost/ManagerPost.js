import React, { useState, useEffect } from 'react'
import { Button, Tooltip } from 'antd'
import {
  PlusOutlined,
  LeftOutlined,
  SaveOutlined,
  SendOutlined
} from '@ant-design/icons'

import './ManagerPost.scss'
import BodyItem from './BodyItem/BodyItem'
import EditorPost from './EditorPost/EditorPost'
import PostDetail from '../PostDetail/PostDetail'
import axios from 'axios'

function ManagerPost () {
  const [isEditor, setIsEditor] = useState(false)
  const [isClickItem, setIsClickItem] = useState(false)
  const [dataContent, setDataContent] = useState('')
  const [room, setRoom] = useState('')
  const [data, setData] = useState()

  useEffect(() => {
    getData()
  }, [])

  /**
   * luu bai
   */
  const savePost = function () {
    var token = localStorage.getItem('access')

    axios
      .put(
        `https://localhost:44342/api/rooms/` + room.roomID,
        {
          roomID: room.roomID,
          userID: room.userID,
          hostID: room.hostID,
          province: room.province,
          district: room.district,
          ward: room.ward,
          address: room.address,
          area: room.area,
          price: room.price,
          status: 3,
          content: dataContent,
          publishDate: new Date(),
          isDeleted: false,
          createdDate: room.createdDate,
          modifiedDate: new Date(),
          createdBy: null,
          modifiedBy: null,
          urlThumbnail: room.urlThumbnail
        },
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      )
      .then(res => {
        console.log(res)
      })
      .catch(error => {
        console.log(error)
      })
  }

  /**
   * dang bai
   */
  const publish = function () {
    var token = localStorage.getItem('access')

    axios
      .put(
        `https://localhost:44342/api/rooms/` + room.roomID,
        {
          roomID: room.roomID,
          userID: room.userID,
          hostID: room.hostID,
          province: room.province,
          district: room.district,
          ward: room.ward,
          address: room.address,
          area: room.area,
          price: room.price,
          status: 4,
          content: dataContent,
          publishDate: new Date(),
          isDeleted: false,
          createdDate: room.createdDate,
          modifiedDate: new Date(),
          createdBy: null,
          modifiedBy: null,
          urlThumbnail: room.urlThumbnail
        },
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      )
      .then(res => {
        console.log(res)
      })
      .catch(error => {
        console.log(error)
      })
  }

  /**
   * l???y danh s??ch ph??ng
   */
  const getData = function () {
    var token = localStorage.getItem('access')
    var hostId = localStorage.getItem('userID')

    axios
      .post(
        `https://localhost:44342/api/rooms/filter`,
        {
          selectedFields: [
            'Price',
            'Address',
            'UrlThumbnail',
            'RoomID',
            'Province',
            'District',
            'Ward',
            'Area',
            'Status',
            'UserID',
            'HostID',
            'Content'
          ],
          filters: [
            [
              {
                key: 'HostID',
                valueArray: [],
                Value: hostId,
                operator: 0
              },
              {
                key: 'Status',
                Value: 0,
                operator: 2
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
        console.log(res)
        console.log(res.data.data)
        setData(res?.data.data)
      })
  }

  return isEditor ? (
    <div className='motel-post'>
      <div className='motel-post__lable'>
        <div>
          <Button
            icon={<LeftOutlined />}
            size='large'
            type='text'
            onClick={() => setIsEditor(false)}
          />
          T???o m???i b??i ????ng
        </div>
        <div>
          <Button
            icon={<SaveOutlined />}
            size='large'
            onClick={() => {
              console.log(dataContent)
              savePost()
            }}
            style={{ marginRight: '8px' }}
          >
            L??u
          </Button>
          <Button
            icon={<SendOutlined />}
            size='large'
            type='primary'
            onClick={() => {
              publish()
            }}
          >
            ????ng b??i
          </Button>
        </div>
      </div>
      <div className='motel-post__content'>
        <EditorPost
          getContent={data => {
            setDataContent(data)
          }}
          getRoom={data => {
            setRoom(data)
          }}
        ></EditorPost>
      </div>
    </div>
  ) : !isClickItem ? (
    <div className='motel-post'>
      <div className='motel-post__lable'>Qu???n l?? b??i ????ng</div>
      <div className='motel-post__content'>
        <Button
          type='primary'
          icon={<PlusOutlined />}
          size='large'
          onClick={() => setIsEditor(true)}
        >
          Th??m b??i ????ng m???i
        </Button>
        <div className='motel-post__content_table'>
          <div className='row header'>
            <div className='row_image header'>H??nh ???nh</div>
            <div className='row_inf header'>Th??ng tin</div>
            <div className='row_content header'>N???i dung</div>
            <div className='row_status header'>Tr???ng th??i</div>
            <div className='row_edit header'>S???a</div>
          </div>

          {data?.map((itemData, key) => {
            return (
              <BodyItem
                onClickItem={() => {
                  // setIsClickItem(true);
                }}
                data={itemData}
              ></BodyItem>
            )
          })}
        </div>
      </div>
    </div>
  ) : (
    <PostDetail
      onClickBack={() => {
        setIsClickItem(false)
      }}
    ></PostDetail>
  )
}

export default ManagerPost
