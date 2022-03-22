import React, { Component, useState } from 'react';
import auth from "../services/auth.service";
import { Formik, Form, Field } from 'formik';
import { RegisterSchema } from "../schemas/register"



export function Register(props) {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [user, setUser] = useState({ username: "", password: "" });

    const [showAlert, setShowAlert] = useState(false);
    const [alertMessage, setAlertMessage] = useState("");
    const [alertType, setAlertType] = useState("alert-danger");

    function handleSubmit(data) {
        register(data.username, data.password);
    }
    function handleChange(event, user) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
        setUser({
            [name]: value
        });
    }
    async function register(username, password) {
        try {
            let response = await auth.register(username, password);
            console.log(response);
            setAlertType("alert-success");
            setAlertMessage("Account Created Successfully")
            setShowAlert(true);
            setUser({ username: "", password: "" });
        }
        catch (err) {
            console.log(err.response.data);
            
            setAlertType("alert-danger");
            const regex = new RegExp('Duplicate')
            if (regex.test(err.response.data))
                setAlertMessage("Username already in use");
            else
                setAlertMessage("Error Occurred")
            setShowAlert(true);

        }
    }

    let alert;
    if (showAlert) {
        alert = <div id="alertPlaceholder" className={`alert ${alertType} alert-dismissible fade show`} role="alert">
            {alertMessage}
            <button type="button" className="btn-close" data-bs-dismiss="alert" aria-label="Close" onClick={()=> setShowAlert(false)}></button>
        </div>;
    }
    return (
        <div>
            <h3>Register</h3>
            {alert}

            <Formik
                initialValues={{
                    username: '',
                    password: ''
                }}
                validationSchema={RegisterSchema}
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
                                <button className="btn btn-success" type="submit">Register</button>
                            </div>
                        </div>
                    </Form>
                )}
            </Formik>
        </div>
    );

}


/*export class Register extends Component {
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

                    <div className="row mb-3 justify-content-center">

                        <div className="col-lg-6">
                            <label htmlFor="username" className="form-label">Username</label>
                            <input type="text" required className="form-control"
                                id="username" aria-describedby="userHelp" name="username"
                                onChange={this.handleChange} value={this.state.username} ></input>
                    </div>
                        <div className="col-lg-6">
                            <label htmlFor="password" className="form-label">Password</label>
                            <input type="password" name="password" id="password" required className="form-control"
                                aria-describedby="pwHelp" onChange={this.handleChange} value={this.state.password} ></input>
                        </div>


                </div>
                    <div className="row mb-3">
                        <div className="col-lg-3">
                            <button className="btn btn-success" type="submit">Register</button>
                                </div>
                            </div>
            </form>
            </div>
        );
    }

    async createUser(username, password) {
        try {
            let response = await fetch("api/User/register", {
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
           let responseText = await response.text();
          if (response.status === 200) {
                this.setState({ username : ""});
                this.setState({ password: "" });
                ;
            } else {
               alert("Username must be unique");
               console.log(response.statusText);
            }
        }
            catch (err) {
                console.log(err);
            }
        
    }
}
*/