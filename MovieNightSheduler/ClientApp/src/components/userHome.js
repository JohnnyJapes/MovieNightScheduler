import React, { Component, useState, useEffect } from "react";
import axios from "axios";
import auth from "../services/auth.service";
import api from "../services/api";
import userService from "../services/user.service";
import viewingService from "../services/viewing.service";
import { useNavigate, Redirect, Link } from "react-router-dom";

export function UserHome(props) {

    // const [user, setUser] = useState(auth.getCurrentUser())
    const [user2, setUser2] = useState();
    const [isBusy, setBusy] = useState(true);
    const [viewings, setViewings] = useState();
    let navigate = useNavigate();
    let response;
    let views = [];
    let response2
    useEffect(() => {
        let loggedIn = true;
        if (props.user == null) { navigate("/"); return; }
        console.log(props.user);
        const getUser = async () => {

            response = await userService.getUserInfo(props.user.id)
            console.log(response);
            console.log(JSON.stringify(response.data));
            await setUser2(response.data);
            console.log(user2);
            if (response.data.groups.length > 0) {
                response.data.groups.forEach(id => {
                    response2 = viewingService.getViewingByGroupID(id)
                    // let response2 = await api.get({
                    //     url: "/Viewing/",
                    //     data: {
                    //         groupId: id
                    //     }
                    // })
                    views.push(response2.data);
                });
                setViewings(views);

                // let response2 = await api.get({
                //     url: "/Viewing/",
                //     data: {
                //         groupId: props.gorups
                //     }
                // });
                // response.data.groups.viewings = {}
            }
            if (loggedIn)
                setBusy(false);
        }
        getUser()
            // response = userService.getUserInfo(user.id)
            //     .then(response => {
            //         console.log(response);
            //         console.log(JSON.stringify(response.data));
            //         setUser2(response.data);
            //         setBusy(false);
            //     })
            .catch((error) => {
                console.error(error);
                navigate("/");
            });
        return () => loggedIn = false;
    }, [props.user])

    if (!isBusy) {


        return (
            <div>
                <h1>{user2.username}'s Home</h1>
                <h3>Currently Part of Groups:</h3>
                <List groups={user2.groups} />
                <h3>Upcoming Viewings:</h3>
                <List groups={user2.groups} />
                <h5>{props.user.id}</h5>
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
        return <li key={group.id}><Link to={"/Group/" + group.id}>{group.name}</Link></li>;
    })
    return (<ul>{groupList}</ul>)
}
// function ListViewings(props) {
//     const viewings = props.groups.viewings;
//     const groupList = groups.map((group) => {
//         console.log(group.name);
//         return <li key={group.id}>{group.name}</li>;
//     })
//     return (<ul>{groupList}</ul>)
// }



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