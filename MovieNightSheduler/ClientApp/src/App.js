import React, { Component, useState, useEffect } from 'react';
import { Route, Routes } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { Register } from './components/Register';
import { Login } from './components/Login';
import { UserHome } from './components/userHome';
import { NotFound } from './components/NotFound';
import auth from "./services/auth.service"

import './custom.css'


export default function App() {
  const displayName = App.name;
  const [user, setUser] = useState(auth.getCurrentUser())
  return (
    <Routes>
      <Route element={<Layout user={user} setUser={setUser} />}>

        <Route path='/' element={<Home />} />
        <Route path='/home' element={<UserHome user={user} setUser={setUser} />} />
        <Route path='/register' element={<Register />} />
        <Route path='/login' element={<Login
          user={user}
          setUser={setUser}
        />} />
        <Route path="*" element={<NotFound />} />

      </Route>
    </Routes>
  );
}