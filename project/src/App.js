import React from 'react'
import PropTypes from 'prop-types'
import HomePage from './template/homePage/HomePage'
import { Switch, Redirect, Route } from 'react-router-dom'
import Revenue from './template/component/Revenue/Revenue'
import Invoice from './template/component/Invoice/Invoice'
import MyMotel from './template/component/myMotel/MyMotel'
import ManagerRoom from './template/component/ManagerRoom/ManagerRoom'
import ManagerPost from './template/component/ManagerPost/ManagerPost'
import ManagerInvoice from './template/component/ManagerInvoice/ManagerInvoice '
import ListMotel from './template/component/listMotel/ListMotel'
import MenuCustom from './template/component/Menu/MenuCustom'
import logo from './styles/Image/logo.png'
import NotFound from './template/component/NotFound/index'
import SettingAcount from './template/component/SettingAcount/SettingAcount'
import ManagerCustomer from './template/component/ManagerCustomer/ManagerCustomer'

App.propTypes = {}

function App (props) {
  return (
    <div>
      <div className='motel-homePage'>
        <div className='motel-homePage__body'>
          <div className='motel-homePage__body_menu'>
            <div className='motel-header__left'>
              <img src={logo} alt='Logo'></img>
              <div> Thuê trọ</div>
            </div>
            <MenuCustom />
          </div>
          <div className='motel-homePage__body_content'>
            <Switch>
              <Redirect from='/home' to='/' exact />

              {/* <Route path='/login' component={Login} exact /> */}

              {/* Thống kê doanh thu */}
              <Route path='/revenue' component={Revenue} exact />
              {/* Hóa đơn => Người thuê*/}
              <Route path='/invoice' component={Invoice} exact />
              {/* Phòng của tôi */}
              <Route path='/motel' component={MyMotel} exact />
              {/* Quản lý phòng trọ */}
              <Route path='/manager-room' component={ManagerRoom} exact />
              {/* Quản lý bài đăng */}
              <Route path='/manager-post' component={ManagerPost} exact />
              {/* Quản lý Hóa đơn => Chủ trọ*/}
              <Route path='/manager-invoice' component={ManagerInvoice} exact />
              {/* Nhận/trả khách trọ => Chủ trọ*/}
              <Route
                path='/manager-customer'
                component={ManagerCustomer}
                exact
              />

              <Route path='/my-account' component={SettingAcount} exact />

              <Route path='/' component={ListMotel} exact />
              <Route path='/not-found' component={NotFound} exact />
            </Switch>
          </div>
        </div>
      </div>
    </div>
  )
}

export default App
