import React, { Component, useState, useEffect } from "react";
import axios from "axios";
import auth from "../services/auth.service";
import api from "../services/api";
import userService from "../services/user.service";


export function UserHome(props) {

    const [user, setUser] = useState(auth.getCurrentUser())
    const [user2, setUser2] = useState();
    const [isBusy, setBusy] = useState(true);
    let response;
    useEffect(() => {
        response = userService.getUserInfo(user.id)
            .then(response => {
                console.log(response);
                console.log(JSON.stringify(response.data));
                setUser2(response.data);
                setBusy(false);
            });

    }, [user])

    if (!isBusy) {


        return (
            <div>
                <h1>{user2.username}'s Home</h1>
                <h3>Currently Part of Groups:</h3>
                <List groups={user2.groups} />

            </div>
        )
    }
    return (
        <div> </div>


    )
}
function List(props) {
    const groups = props.groups;
    const groupList = groups.map((group) => {
        console.log(group.name);
        return <li key={group.id}>{group.name}</li>;
    })
    return (<ul>{groupList}</ul>)
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