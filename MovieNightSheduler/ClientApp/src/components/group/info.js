import React, { Component, useState, useEffect } from "react";
import axios from "axios";
import auth from "../../services/auth.service";
import api from "../../services/api";
import groupService from "../../services/group.service";
import { useNavigate, Redirect, useParams } from "react-router-dom";

export function GroupInfo(props) {

    const [group, setGroup] = useState();
    const [isBusy, setBusy] = useState(true);
    const [viewings, setViewings] = useState();
    let { id } = useParams();
    let navigate = useNavigate();
    let response;
    useEffect(() => {
        let start = true;
        if (props.user == null) { navigate("/"); return; }
        console.log(props.user);
        const getGroup = async () => {

            response = await groupService.getGroup(id)
            console.log(response);
            console.log(JSON.stringify(response.data));
            setGroup(response.data);
            if (start)
                setBusy(false);
        }
        getGroup()
            // response = userService.getUserInfo(user.id)
            //     .then(response => {
            //         console.log(response);
            //         console.log(JSON.stringify(response.data));
            //         setUser2(response.data);
            //         setBusy(false);
            //     })
            .catch((error) => {
                console.error(error);
                //navigate("/");
            });
        return () => start = false;
    }, [props.user])

    if (!isBusy) {


        return (
            <div>
                <h1>{group.name}'s Home</h1>
                <h3>Upcoming Viewings:</h3>
                {/* <List groups={user2.groups} />
                <h3>Upcoming Viewings:</h3>
                <List groups={user2.groups} />
                <h5>{props.user.id}</h5> */}
            </div>

        )
    }
    else return (
        <div>Error Occured</div>
    )


}