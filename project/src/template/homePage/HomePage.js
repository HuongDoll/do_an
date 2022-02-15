import React, { useState, useEffect } from 'react'
import Header from '../header/Header'
import background from './../../styles/Image/background.jpg'
import IconHome from './../../styles/Icon/home.png'
import IconRoom from './../../styles/Icon/room.png'
import IconManagerRoom from './../../styles/Icon/manager_room.png'
import IconPost from './../../styles/Icon/post.png'
import IconRent from './../../styles/Icon/rent.png'
import IconCustomer from './../../styles/Icon/customer.png'
import IconInvoice from './../../styles/Icon/invoice.png'
import IconRevenue from './../../styles/Icon/revenue.png'

import 'antd/dist/antd.css'
import './homePage.scss'
import ListMotel from '../component/listMotel/ListMotel'
import ManagerCustomer from '../component/ManagerCustomer/ManagerCustomer'
import ManagerInvoice from '../component/ManagerInvoice/ManagerInvoice '
import ManagerPost from '../component/ManagerPost/ManagerPost'
import ManagerRoom from '../component/ManagerRoom/ManagerRoom'
import MyMotel from '../component/myMotel/MyMotel'
import Revenue from '../component/Revenue/Revenue'
import Invoice from '../component/Invoice/Invoice'
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
  UserOutlined
} from '@ant-design/icons'
import SettingAcount from '../component/SettingAcount/SettingAcount'

const MENU_INDEX = {
  HOME: 1,
  MY_ROOM: 2,
  MANAGEMENT_ROOM: 3,
  MANAGEMENT_POST: 4,
  MANAGEMENT_RENTAL: 5,
  CUSTOMER: 6,
  MANAGEMENT_INVOICE: 7,
  REVENUE: 8,
  INVOICE: 9,
  POST_FAVORITE: 10,
  ACOUNT: 11
}

function HomePage () {
  const [index, setIndex] = useState(MENU_INDEX.HOME)
  const [islogin, setislogin] = useState(false)
  const [typeUser, setTypeUser] = useState(0)
  const [openLogin, setOpenLogin] = useState(false)

  useEffect(() => {
    setislogin(localStorage.getItem('login'))
    setTypeUser(localStorage.getItem('usertype'))
  }, [])

  localStorage.getItem('login')

  return (
    <div className='motel-homePage'>
      <Header
        onlogin={() => {
          setislogin(localStorage.getItem('login'))
          setTypeUser(localStorage.getItem('usertype'))
        }}
        openlogin={openLogin}
      ></Header>
      <div className='motel-homePage__body'>
        <div className='motel-homePage__body_menu'>
          <div
            className={
              'motel-homePage__body_menu_item ' +
              (index === MENU_INDEX.HOME ? 'select-menu' : '')
            }
            onClick={() => {
              setIndex(MENU_INDEX.HOME)
            }}
          >
            <HomeOutlined
              style={{
                fontSize: '24px',
                marginRight: '12px',
                color: index === MENU_INDEX.HOME ? '#2a85da' : '#fff'
              }}
            />

            <span>Trang chủ</span>
          </div>
          {typeUser == 0 && (
            <>
              <div
                className={
                  'motel-homePage__body_menu_item ' +
                  (index === MENU_INDEX.POST_FAVORITE ? 'select-menu' : '')
                }
                onClick={() => {
                  if (islogin == 1) setIndex(MENU_INDEX.POST_FAVORITE)
                  else setOpenLogin(!openLogin)
                }}
              >
                <HeartOutlined
                  style={{
                    fontSize: '24px',
                    marginRight: '12px',
                    color:
                      index === MENU_INDEX.POST_FAVORITE ? '#2a85da' : '#fff'
                  }}
                />

                <span>Yêu thích</span>
              </div>
              <div
                className={
                  'motel-homePage__body_menu_item ' +
                  (index === MENU_INDEX.MY_ROOM ? 'select-menu' : '')
                }
                onClick={() => {
                  if (islogin == 1) setIndex(MENU_INDEX.MY_ROOM)
                  else setOpenLogin(!openLogin)
                }}
              >
                <GroupOutlined
                  style={{
                    fontSize: '24px',
                    marginRight: '12px',
                    color: index === MENU_INDEX.MY_ROOM ? '#2a85da' : '#fff'
                  }}
                />

                <span>Phòng của tôi</span>
              </div>
              <div
                className={
                  'motel-homePage__body_menu_item ' +
                  (index === MENU_INDEX.INVOICE ? 'select-menu' : '')
                }
                onClick={() => {
                  if (islogin == 1) setIndex(MENU_INDEX.INVOICE)
                  else setOpenLogin(!openLogin)
                }}
              >
                <SnippetsOutlined
                  style={{
                    fontSize: '24px',
                    marginRight: '12px',
                    color: index === MENU_INDEX.INVOICE ? '#2a85da' : '#fff'
                  }}
                />

                <span>Hóa đơn</span>
              </div>
            </>
          )}
          {typeUser == 1 && islogin == 1 && (
            <>
              <div
                className={
                  'motel-homePage__body_menu_item ' +
                  (index === MENU_INDEX.MANAGEMENT_ROOM ? 'select-menu' : '')
                }
                onClick={() => {
                  setIndex(MENU_INDEX.MANAGEMENT_ROOM)
                }}
              >
                <WalletOutlined
                  style={{
                    fontSize: '24px',
                    marginRight: '12px',
                    color:
                      index === MENU_INDEX.MANAGEMENT_ROOM ? '#2a85da' : '#fff'
                  }}
                />

                <span>Quản lý phòng trọ</span>
              </div>
              <div
                className={
                  'motel-homePage__body_menu_item ' +
                  (index === MENU_INDEX.MANAGEMENT_POST ? 'select-menu' : '')
                }
                onClick={() => {
                  setIndex(MENU_INDEX.MANAGEMENT_POST)
                }}
              >
                <FormOutlined
                  style={{
                    fontSize: '24px',
                    marginRight: '12px',
                    color:
                      index === MENU_INDEX.MANAGEMENT_POST ? '#2a85da' : '#fff'
                  }}
                />

                <span>Quản lý bài đăng</span>
              </div>
              <div
                className={
                  'motel-homePage__body_menu_item ' +
                  (index === MENU_INDEX.CUSTOMER ? 'select-menu' : '')
                }
                onClick={() => {
                  setIndex(MENU_INDEX.CUSTOMER)
                }}
              >
                <SolutionOutlined
                  style={{
                    fontSize: '24px',
                    marginRight: '12px',
                    color: index === MENU_INDEX.CUSTOMER ? '#2a85da' : '#fff'
                  }}
                />

                <span>Nhận/trả khách trọ</span>
              </div>
              <div
                className={
                  'motel-homePage__body_menu_item ' +
                  (index === MENU_INDEX.MANAGEMENT_INVOICE ? 'select-menu' : '')
                }
                onClick={() => {
                  setIndex(MENU_INDEX.MANAGEMENT_INVOICE)
                }}
              >
                <SnippetsOutlined
                  style={{
                    fontSize: '24px',
                    marginRight: '12px',
                    color:
                      index === MENU_INDEX.MANAGEMENT_INVOICE
                        ? '#2a85da'
                        : '#fff'
                  }}
                />

                <span>Quản lý hóa đơn</span>
              </div>
              <div
                className={
                  'motel-homePage__body_menu_item ' +
                  (index === MENU_INDEX.REVENUE ? 'select-menu' : '')
                }
                onClick={() => {
                  setIndex(MENU_INDEX.REVENUE)
                }}
              >
                <AreaChartOutlined
                  style={{
                    fontSize: '24px',
                    marginRight: '12px',
                    color: index === MENU_INDEX.REVENUE ? '#2a85da' : '#fff'
                  }}
                />

                <span>Thống kê doanh thu</span>
              </div>
            </>
          )}
          <div
            className={
              'motel-homePage__body_menu_item ' +
              (index === MENU_INDEX.ACOUNT ? 'select-menu' : '')
            }
            onClick={() => {
              if (islogin == 1) setIndex(MENU_INDEX.ACOUNT)
              else setOpenLogin(!openLogin)
            }}
          >
            <UserOutlined
              style={{
                fontSize: '24px',
                marginRight: '12px',
                color: index === MENU_INDEX.ACOUNT ? '#2a85da' : '#fff'
              }}
            />

            <span>Tài khoản</span>
          </div>
        </div>
        <div className='motel-homePage__body_content'>
          {index === MENU_INDEX.HOME ? <ListMotel></ListMotel> : <></>}
          {index === MENU_INDEX.CUSTOMER ? (
            <ManagerCustomer></ManagerCustomer>
          ) : (
            <></>
          )}
          {index === MENU_INDEX.MANAGEMENT_INVOICE ? (
            <ManagerInvoice></ManagerInvoice>
          ) : (
            <></>
          )}
          {index === MENU_INDEX.MANAGEMENT_POST ? (
            <ManagerPost></ManagerPost>
          ) : (
            <></>
          )}

          {index === MENU_INDEX.MANAGEMENT_ROOM ? (
            <ManagerRoom></ManagerRoom>
          ) : (
            <></>
          )}
          {index === MENU_INDEX.MY_ROOM ? <MyMotel></MyMotel> : <></>}
          {index === MENU_INDEX.REVENUE ? <Revenue></Revenue> : <></>}
          {index === MENU_INDEX.INVOICE ? <Invoice></Invoice> : <></>}
          {index === MENU_INDEX.ACOUNT ? (
            <SettingAcount></SettingAcount>
          ) : (
            <></>
          )}
        </div>
      </div>
    </div>
  )
}

export default HomePage
