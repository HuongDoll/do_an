import React, { useState, useEffect } from 'react'
import PropTypes from 'prop-types'
import { Modal, Menu, Button, notification, Radio, Input } from 'antd'
import {
  AppstoreOutlined,
  MenuUnfoldOutlined,
  MenuFoldOutlined,
  PieChartOutlined,
  DesktopOutlined,
  ContainerOutlined,
  MailOutlined,
  HomeOutlined,
  GroupOutlined,
  SnippetsOutlined,
  WalletOutlined,
  FormOutlined,
  SolutionOutlined,
  AreaChartOutlined,
  HeartOutlined,
  UserOutlined,
  UserAddOutlined,
  UserDeleteOutlined,
  SmileOutlined,
  UnlockOutlined,
  LogoutOutlined,
  LoginOutlined,
  UsergroupAddOutlined
} from '@ant-design/icons'
import { useHistory } from 'react-router-dom'
import axios from 'axios'
import './MenuCustom.scss'

const { SubMenu } = Menu
MenuCustom.propTypes = {}

function MenuCustom (props) {
  const history = useHistory()
  const [isLogin, setIsLogin] = useState(false)

  const [collapsed, setCollapsed] = useState(false)
  const [modalLogin, setModalLogin] = useState(false)
  const [modalRes, setModalRes] = useState(false)
  const [typeUser, setTypeUser] = useState(0)

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
    setIsLogin(localStorage.getItem('login') == 1)
    setName(localStorage.getItem('fullName')?.toString() || 'Tài khoản')
    setTypeUser(localStorage.getItem('usertype') || 0)
  }, [])

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

        setName(res?.data?.fullName)
        setTypeUser(res?.data?.userType)
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
    // props.onlogin()

    setName('Tài khoản')

    setIsLogin(false)
    setTypeUser(0)
  }

  return (
    <div style={{ width: '100%' }}>
      <Button
        type='primary'
        onClick={() => {
          setCollapsed(!collapsed)
        }}
        style={{ marginBottom: 16, display: 'none' }}
      >
        {React.createElement(collapsed ? MenuUnfoldOutlined : MenuFoldOutlined)}
      </Button>
      <Menu
        defaultSelectedKeys={['1']}
        defaultOpenKeys={['sub1']}
        mode='inline'
        theme='light'
        inlineCollapsed={collapsed}
        className='motel-menu'
      >
        <Menu.Item
          key='1'
          icon={<HomeOutlined style={{ fontSize: '20px' }} />}
          onClick={() => {
            history.push('/')
          }}
        >
          Trang chủ
        </Menu.Item>
        {typeUser == 0 && (
          <>
            <Menu.Item
              key='2'
              icon={<GroupOutlined style={{ fontSize: '20px' }} />}
              onClick={() => {
                if (isLogin == 1) history.push('/motel')
                else setModalLogin(true)
              }}
            >
              Phòng của tôi
            </Menu.Item>
            <Menu.Item
              key='3'
              icon={<SnippetsOutlined style={{ fontSize: '20px' }} />}
              onClick={() => {
                if (isLogin == 1) history.push('/invoice')
                else setModalLogin(true)
              }}
            >
              Hóa đơn
            </Menu.Item>
          </>
        )}
        {typeUser == 1 && isLogin == 1 && (
          <>
            <Menu.Item
              key='4'
              icon={<WalletOutlined style={{ fontSize: '20px' }} />}
              onClick={() => {
                history.push('/manager-room')
              }}
            >
              Quản lý phòng trọ
            </Menu.Item>
            <Menu.Item
              key='5'
              icon={<FormOutlined style={{ fontSize: '20px' }} />}
              onClick={() => {
                history.push('/manager-post')
              }}
            >
              Quản lý bài đăng
            </Menu.Item>
            <SubMenu
              key='sub1'
              icon={<SolutionOutlined style={{ fontSize: '20px' }} />}
              title='Nhận/trả khách trọ'
            >
              <Menu.Item
                key='6'
                icon={<UserAddOutlined style={{ fontSize: '18px' }} />}
                onClick={() => {
                  history.push('/manager-customer')
                }}
              >
                Yêu cầu thuê trọ
              </Menu.Item>
              <Menu.Item
                key='7'
                icon={<UserDeleteOutlined style={{ fontSize: '18px' }} />}
                onClick={() => {
                  history.push('/manager-customer')
                }}
              >
                Yêu cầu trả trọ
              </Menu.Item>
            </SubMenu>
            <Menu.Item
              key='8'
              icon={<SnippetsOutlined style={{ fontSize: '20px' }} />}
              onClick={() => {
                history.push('/manager-invoice')
              }}
            >
              Quản lý hóa đơn
            </Menu.Item>
            <Menu.Item
              key='9'
              icon={<DesktopOutlined style={{ fontSize: '20px' }} />}
              onClick={() => {
                history.push('/revenue')
              }}
            >
              Thống kê doanh thu
            </Menu.Item>
          </>
        )}

        <SubMenu
          key='sub2'
          icon={<SmileOutlined style={{ fontSize: '20px' }} />}
          title={name}
        >
          {isLogin ? (
            <>
              <Menu.Item
                key='10'
                onClick={() => {
                  history.push('/my-account')
                }}
                icon={<UserOutlined style={{ fontSize: '18px' }} />}
              >
                Thông tin cá nhân
              </Menu.Item>
              <Menu.Item
                key='11'
                icon={<UnlockOutlined style={{ fontSize: '18px' }} />}
              >
                Mật khẩu
              </Menu.Item>
              <Menu.Item
                key='12'
                onClick={() => {
                  logout()
                }}
                icon={<LogoutOutlined style={{ fontSize: '18px' }} />}
              >
                Đăng xuất
              </Menu.Item>
            </>
          ) : (
            <>
              <Menu.Item
                key='13'
                onClick={() => {
                  setModalLogin(true)
                }}
                icon={<LoginOutlined style={{ fontSize: '18px' }} />}
              >
                Đăng nhập
              </Menu.Item>
              <Menu.Item
                key='14'
                onClick={() => {
                  setModalRes(true)
                }}
                icon={<UsergroupAddOutlined style={{ fontSize: '18px' }} />}
              >
                Đăng ký
              </Menu.Item>
            </>
          )}
        </SubMenu>
      </Menu>
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
    </div>
  )
}

export default MenuCustom
