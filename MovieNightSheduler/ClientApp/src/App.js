import React, { Component } from 'react';
import { Route, Routes } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { Register } from './components/Register';
import { Login } from './components/Login'
import {UserHome} from './components/userHome'

import './custom.css'

export default function App() {
  const displayName = App.name;


    return (
        <Layout>
            <Routes>
                <Route path='/' element={<UserHome />} />
                <Route path='/register' element={<Register />}/>
                <Route path='/login' element={<Login />}/>
            </Routes>
      </Layout>
    );
  }