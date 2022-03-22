import React, { Component, useState } from "react";
import axios from "axios";
import auth from "../services/auth.service";


export function UserHome(props) {

    const [user, setUser] = useState(auth.getCurrentUser())
    return (
        <div>
            <h1>{user.username}'s Home</h1>


        </div>


    )
}


/*export class UserHome extends Component {
    componentDidMount() {
        auth.refreshToken();
    }
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
}*/