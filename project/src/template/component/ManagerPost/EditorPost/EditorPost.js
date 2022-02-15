import React, { useState, useEffect } from 'react'
import { CKEditor } from '@ckeditor/ckeditor5-react'
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import { Select } from 'antd'
import './EditorPost.scss'
import axios from 'axios'

function EditorPost (props) {
  const { Option } = Select
  const [dataRoom, setDataRoom] = useState()

  useEffect(() => {
    getData()
  }, [])

  /**
   * lấy danh sách phòng
   */
  const getData = function (props) {
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
            'HostID'
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
                valueArray: [],
                Value: 0,
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
        console.log(res)
        console.log(res.data.data)
        // setData(res?.data.data)
        setDataRoom(res?.data?.data)
      })
  }

  return (
    <div className='editor-post'>
      <div className='editor-post__room'>
        <h2>Chọn phòng</h2>

        <Select
          showSearch
          style={{ width: 200 }}
          placeholder='Chọn phòng'
          optionFilterProp='children'
          filterOption={(input, option) =>
            option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
          }
          filterSort={(optionA, optionB) =>
            optionA.children
              .toLowerCase()
              .localeCompare(optionB.children.toLowerCase())
          }
          onSelect={value => {
            props?.getRoom(dataRoom[value])
          }}
        >
          {dataRoom?.map((dataItem, key) => {
            return <Option value={key}>{dataItem.address}</Option>
          })}
        </Select>
      </div>
      <h2>Chỉnh sửa nội dung bài đăng</h2>
      <CKEditor
        editor={ClassicEditor}
        data='<p>Nội dung bài đăng...</p>'
        onReady={editor => {
          // You can store the "editor" and use when it is needed.
          console.log('Editor is ready to use!', editor)
        }}
        onChange={(event, editor) => {}}
        onBlur={(event, editor) => {
          props?.getContent(editor.getData())
        }}
        onFocus={(event, editor) => {}}
      />
    </div>
  )
}

export default EditorPost
