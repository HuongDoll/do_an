import React, { useState, useEffect } from 'react'
import logo from './../../styles/Image/logo.png'
import avt from './../../styles/Image/avt.png'
import { Menu, Dropdown, message, Input, EyeTwoTone } from 'antd'
import { Modal, Button, notification, Radio } from 'antd'
import { SettingOutlined, LogoutOutlined } from '@ant-design/icons'
import PropTypes from 'prop-types'

import axios from 'axios'

import './header.scss'

Header.propTypes = {
  onlogin: PropTypes.func,
  openlogin: PropTypes.bool
}

Header.defaultProps = {
  onlogin: () => {},
  openlogin: false
}

function Header (props) {
  const [isLogin, setIsLogin] = useState(false)
  const [modalLogin, setModalLogin] = useState(false)
  const [modalRes, setModalRes] = useState(false)

  const [user, setUser] = useState({
    fullName: '',
    userName: '',
    password: '',
    email: '',
    phone: '',
    passwordConfirm: '',
    userType: 0
  })

  const [name, setName] = useState('')

  useEffect(() => {
    console.log(localStorage.getItem('login'))
    console.log(isLogin)

    setIsLogin(localStorage.getItem('login') == 1)
    console.log(isLogin)

    setName(localStorage.getItem('fullName')?.toString())
  }, [])

  useEffect(() => {
    setModalLogin(true)
  }, [props.openlogin])

  const onClick = ({ key }) => {
    if (key === '2') logout()
  }

  const menu = (
    <Menu onClick={onClick}>
      <Menu.Item key='1'>
        <SettingOutlined style={{ marginRight: '8px' }} />
        Cài đặt tài khoản
      </Menu.Item>
      <Menu.Item key='2'>
        <LogoutOutlined style={{ marginRight: '8px' }} />
        Đăng xuất
      </Menu.Item>
    </Menu>
  )

  /**
   * tao tai khoan
   */
  const clickResignter = function () {
    axios
      .post(`https://localhost:44342/api/users`, {
        fullName: user.fullName,
        userName: user.userName,
        password: user.password,
        email: user.email,
        phone: user.phone,
        userType: user.userType
      })
      .then(res => {
        console.log(res)
        console.log(res.data)
        setModalRes(false)
        setModalLogin(true)
      })
  }

  /**
   * dang nhap
   */
  const clickLogin = function () {
    axios
      .post(`https://localhost:44342/api/users/login`, {
        userName: user.userName,
        password: user.password
      })
      .then(res => {
        console.log(res?.data?.fullName)
        console.log(res.data)
        setIsLogin(true)
        setModalLogin(false)

        notification.open({
          message: 'Đăng nhập thành công',
          description: 'Xin chào ' + res?.data?.fullName,
          onClick: () => {
            console.log('Notification Clicked!')
          }
        })

        localStorage.setItem('fullName', res?.data?.fullName)
        localStorage.setItem('userID', res?.data?.userID)
        localStorage.setItem('access', res?.data?.access)
        localStorage.setItem('login', 1)
        localStorage.setItem('usertype', res?.data?.userType)
        props.onlogin()
        setName(res?.data?.fullName)
      })
  }

  /**
   * dang  xuat
   */
  const logout = function () {
    localStorage.setItem('fullName', '')
    localStorage.setItem('userID', '')
    localStorage.setItem('access', '')
    localStorage.setItem('login', 0)
    localStorage.setItem('usertype', 0)
    props.onlogin()

    setIsLogin(false)
  }

  return (
    <div className='motel-header'>
      <div className='motel-header__left'>
        <img src={logo} alt='Logo'></img>
        <div> Thuê trọ</div>
      </div>

      <div className='motel-header__right'>
        {isLogin ? (
          <>
            <Dropdown overlay={menu}>
              <div
                className='ant-dropdown-link motel-header__right_name
'
                onClick={e => e.preventDefault()}
              >
                {name?.toString()}
                <img
                  className='motel-header__right_avt'
                  src={avt}
                  alt='avt'
                ></img>
              </div>
            </Dropdown>
          </>
        ) : (
          <>
            <Button
              onClick={() => {
                setModalLogin(true)
              }}
            >
              Đăng nhập
            </Button>
            <Button
              type='primary'
              onClick={() => {
                setModalRes(true)
              }}
            >
              Đăng ký
            </Button>
            <Modal
              title='Đăng nhập'
              style={{ top: 20 }}
              visible={modalLogin}
              onOk={() => {
                clickLogin()
              }}
              onCancel={() => setModalLogin(false)}
            >
              <p>Tên đăng nhập (*)</p>
              <Input
                placeholder='Tên đăng nhập'
                style={{ marginBottom: '16px' }}
                required
                onChange={value => {
                  setUser({ ...user, userName: value.target.value })
                }}
              />

              <p>Mật khẩu (*)</p>
              <Input.Password
                placeholder='Mật khẩu'
                onChange={value => {
                  setUser({ ...user, password: value.target.value })
                }}
              />
            </Modal>
            <Modal
              title='Đăng ký'
              style={{ top: 20 }}
              visible={modalRes}
              onOk={() => {
                clickResignter()
              }}
              onCancel={() => setModalRes(false)}
            >
              <p>Họ và tên (*)</p>
              <Input
                placeholder='Họ và tên '
                style={{ marginBottom: '16px' }}
                required
                defaultValue={user.fullName}
                onChange={value => {
                  setUser({ ...user, fullName: value.target.value })
                }}
              />
              <p>Số điện thoại (*)</p>
              <Input
                placeholder='Số điện thoại'
                style={{ marginBottom: '16px' }}
                required
                defaultValue={user.phone}
                onChange={value => {
                  setUser({ ...user, phone: value.target.value })
                }}
              />
              <p>Email</p>
              <Input
                placeholder='Email'
                style={{ marginBottom: '16px' }}
                required
                defaultValue={user.email}
                onChange={value => {
                  setUser({ ...user, email: value.target.value })
                }}
              />

              <p>Tên đăng nhập</p>
              <Input
                placeholder='Tên đăng nhập'
                style={{ marginBottom: '16px' }}
                required
                defaultValue={user.userName}
                onChange={value => {
                  setUser({ ...user, userName: value.target.value })
                }}
              />

              <p>Mật khẩu (*)</p>
              <Input.Password
                placeholder='Mật khẩu'
                style={{ marginBottom: '16px' }}
                onChange={value => {
                  setUser({ ...user, password: value.target.value })
                }}
              />
              <p>Xác nhận mật khẩu (*)</p>
              <Input.Password
                placeholder='Mật khẩu'
                style={{ marginBottom: '16px' }}
                onChange={value => {
                  setUser({ ...user, passwordConfirm: value.target.value })
                }}
              />
              <p>Loại tài khoản</p>
              <Radio.Group
                onChange={value => {
                  setUser({ ...user, userType: value.target.value })
                }}
                defaultValue={0}
              >
                <Radio value={0}>Người thuê</Radio>
                <Radio value={1}>Chủ trọ</Radio>
              </Radio.Group>
            </Modal>
          </>
        )}
      </div>
    </div>
  )
}

export default Header
