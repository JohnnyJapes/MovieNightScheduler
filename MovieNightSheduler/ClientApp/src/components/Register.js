﻿import React, { Component} from 'react';


export class Register extends Component {
    static displayName = Register.name;

     

    constructor(props) {
        super(props);
        this.state = {
            username: "",
            password: ""
        };
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        
    }

     handleSubmit = async (event) => {
        event.preventDefault();
        console.log(event.target.username.value + " " + event.target.password.value);
        this.createUser(event.target.username.value, event.target.password.value)
    }

    handleChange= async(event) => {
        const target = event.target;
        const value = target.value;
        const name = target.name;
        this.setState({
            [name]: value
        });
    }



    render() {
        return (
            <div>
                <form onSubmit={this.handleSubmit}>

                <div class="row mb-3 justify-content-center">

                    <div class="col-lg-6">
                        <label for="username" class="form-label">Username</label>
                            <input type="text" required class="form-control"
                                id="username" aria-describedby="userHelp" name="username"
                                onChange={this.handleChange} value={this.state.username} ></input>
                    </div>
                        <div class="col-lg-6">
                            <label for="password" class="form-label">Password</label>
                            <input type="password" name="password" id="password" required class="form-control"
                                aria-describedby="pwHelp" onChange={this.handleChange} value={this.state.password} ></input>
                        </div>


                </div>
                            <div class="row mb-3">
                                <div class="col-lg-3">
                                    <button class="btn btn-success" type="submit">Register</button>
                                </div>
                            </div>
            </form>
            </div>
        );
    }

    async createUser(username, password) {
        try {
            let response = await fetch("api/User", {
                method: "POST",
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    username: username,
                    password: password
                })
            });
            console.log(JSON.stringify({
                Username: username,
                password: password
            }));
            let responseText = await response.text();
            if (response.status === 200) {
                this.setState({ username : ""});
                this.setState({ password: "" });
                ;
            } else {
               alert("Username must be unique");
               console.log("error");
            }
        }
            catch (err) {
                console.log(err);
            }
        
    }
}