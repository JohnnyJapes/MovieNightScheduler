import React, { Component } from "react";
import axios from "axios";

export class UserHome extends Component {
    static displayName = UserHome.name;
    constructor(props) {
        super(props);
        this.state = {
            user: JSON.parse(localStorage.getItem('user'))
        };
        console.log(this.state.username)
    }


    render() {
        return (
            <div>
                <h1>{this.state.user.username}'s Home</h1>


            </div>

            
            )
    }
}