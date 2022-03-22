import React, { Component, useState } from "react";
import axios from "axios";
import auth from "../services/auth.service"
import { LoginSchema } from "../schemas/login"
import { Formik, Form, Field } from 'formik';

export function Login(props) {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [user, setUser] = useState({ username: "", password: "" });

    const [showAlert, setShowAlert] = useState(false);
    const [alertMessage, setAlertMessage] = useState("");
    const [alertType, setAlertType] = useState("alert-danger");

    function handleSubmit(data) {
        login(data.username, data.password);
    }
    async function login(username, password) {
        try {
            let response = await auth.login(username, password);
            console.log(response);
            setAlertType("alert-success");
            setAlertMessage("Login Successful")
            setShowAlert(true);
            setUser({ username: "", password: "" });
        }
        catch (err) {
            console.log(err.response.data.message);

            setAlertType("alert-danger");
            setAlertMessage("Invalid Username Or Password");
            setShowAlert(true);

        }

    }
    let alert;
    if (showAlert) {
        alert = <div id="alertPlaceholder" className={`alert ${alertType} alert-dismissible fade show`} role="alert">
            {alertMessage}
            <button type="button" className="btn-close" data-bs-dismiss="alert" aria-label="Close" onClick={() => setShowAlert(false)}></button>
        </div>;
    }
    return (

        <div>
            <h3>Login</h3>
            {alert}

            <Formik
                initialValues={{
                    username: '',
                    password: ''
                }}
                validationSchema={LoginSchema}
                onSubmit={handleSubmit}
            >
                {({ errors, touched }) => (
                    <Form>

                        <div className="row mb-3 justify-content-center">

                            <div className="col-lg-6">
                                <label htmlFor="username" className="form-label">Username</label>
                                <Field type="text" required className="form-control"
                                    id="username" aria-describedby="userHelp" name="username" />
                                {/* onChange={this.handleChange} value={this.state.username} ></input>*/}
                                {errors.username && touched.username ? (<div className="text-danger">{errors.username}</div>) : null}

                            </div>
                            <div className="col-lg-6">
                                <label htmlFor="password" className="form-label">Password</label>
                                <Field type="password" name="password" id="password" required className="form-control" />
                                {/* aria-describedby="pwHelp" onChange={this.handleChange} value={this.state.password} ></input>*/}
                                {errors.password && touched.password ? (<div className="text-danger">{errors.password}</div>) : null}
                            </div>


                        </div>
                        <div className="row mb-3">
                            <div className="col-lg-3">
                                <button className="btn btn-success" type="submit">Login</button>
                            </div>
                        </div>
                    </Form>
                )}
            </Formik>
        </div>
    );
}

/*export class Login extends Component{
    static displayName = Login.name;

    constructor(props) {
        super(props);
        this.state = {
            username: "",
            password: "",
            showAlert: false,
            alertMessage: ""
        };
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleCloseAlert = this.handleCloseAlert.bind(this);

    }

    handleSubmit = async (data) => {
*//*        event.preventDefault();
        console.log(event.target.username.value + " " + event.target.password.value);
        console.log(LoginSchema);
        this.login(event.target.username.value, event.target.password.value)*//*
        console.log(data);
        this.login(data.username, data.password)
    }

    handleChange = async (event) => {
        const target = event.target;
        const value = target.value;
        const name = target.name;
        this.setState({
            [name]: value
        });
    }
    handleCloseAlert()  {
        this.setState({ showAlert: false });
    }



    render() {
        const showAlert = this.state.showAlert
        let alert;
        if (showAlert) {
            alert = <div id="alertPlaceholder" className="alert alert-danger alert-dismissible fade show" role="alert">
                {this.state.alertMessage}
                <button type="button" className="btn-close" data-bs-dismiss="alert" aria-label="Close" onClick={this.handleCloseAlert}></button>
                    </div>;
        }
        return (

            <div>
                {alert}

                <Formik
                    initialValues={{
                        username: '',
                        password: ''
                    }}
                    validationSchema={LoginSchema}
                    onSubmit={this.handleSubmit}
                >
                    {({ errors, touched }) => (
                        <Form>

                            <div className="row mb-3 justify-content-center">

                                <div className="col-lg-6">
                                    <label htmlFor="username" className="form-label">Username</label>
                                    <Field type="text" required className="form-control"
                                        id="username" aria-describedby="userHelp" name="username" />
                                    {*//* onChange={this.handleChange} value={this.state.username} ></input>*//*}
                                    {errors.username && touched.username ? (<div className="text-danger">{errors.username}</div>) : null}

                                </div>
                                <div className="col-lg-6">
                                    <label htmlFor="password" className="form-label">Password</label>
                                    <Field type="password" name="password" id="password" required className="form-control" />
                                    {*//* aria-describedby="pwHelp" onChange={this.handleChange} value={this.state.password} ></input>*//*}
                                    {errors.password && touched.password ? (<div className="text-danger">{errors.password}</div>) : null}
                                </div>


                            </div>
                            <div className="row mb-3">
                                <div className="col-lg-3">
                                    <button className="btn btn-success" type="submit">Login</button>
                                </div>
                            </div>
                        </Form>
                    )}
                </Formik>
            </div>
        );
    }
    async login(username, password) {
        try {
            let response = await auth.login(username, password);
            console.log(response);
            this.setState({ username: "" });
            this.setState({ password: "" });
        }
        catch(err) {
            console.log(err.response.data.message);
            this.setState({
                showAlert: true,
                alertMessage: "Invalid Username or Password"
            });

        }





        *//*try {
            let response = await fetch("api/User/authenticate", {
                method: "POST",
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    Username: username,
                    Password: password
                })
            });
            console.log(JSON.stringify({
                Username: username,
                password: password
            }));
            let responseText = await response.json();
            console.log(responseText);
            if (response.status === 200) {
                this.setState({ username: "" });
                this.setState({ password: "" });
                localStorage.setItem("user", JSON.stringify(responseText));
                ;
            } else {
                alert("Username must be unique");
                console.log("error");
            }
        }
        catch (err) {
            console.log(err);
        }*//*

    }
}*/