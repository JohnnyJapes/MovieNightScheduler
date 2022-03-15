import React, { Component } from "react";
import axios from "axios";

export class UserHome extends Component {
    static displayName = UserHome.name;
    constructor(props) {
        super(props);
        let temp = JSON.parse(localStorage.getItem('user'));
        this.state = {
            id: temp.id,
            username: temp.username,
            token: temp.jwtToken
        };
        console.log(this.state.username)
    }


    render() {
        return (
            <div>
                <h1>{this.state.username}'s Home</h1>


            </div>

            
            )
    }
}