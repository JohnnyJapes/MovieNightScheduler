import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import { Outlet } from "react-router-dom";

export function Layout(props) {
  //static displayName = Layout.name;


  return (
    <div>
      <NavMenu user={props.user} setUser={props.setUser} />
      <Container>
        <Outlet />
      </Container>
    </div>
  );
}

