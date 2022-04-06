import React, { Component, useState } from 'react';
import auth from "../services/auth.service";
import { Formik, Form, Field } from 'formik';
import { GroupSchema } from "../schemas/group"
import groupService from "../services/group.service";



export function Register(props) {
    const [name, setUsername] = useState("");
    const [description, setDescription] = useState("");
    const [group, setGroup] = useState({ name: "", description: "" });

    const [showAlert, setShowAlert] = useState(false);
    const [alertMessage, setAlertMessage] = useState("");
    const [alertType, setAlertType] = useState("alert-danger");

    function handleSubmit(data) {
        register(data.name, data.description);
    }
    function handleChange(event, group) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
        setGroup({
            [name]: value
        });
    }
    async function register(name, description) {
        try {
            let response = await groupService.createGroup(name, description);
            console.log(response);
            setAlertType("alert-success");
            setAlertMessage("Group Created Successfully")
            setShowAlert(true);
            setGroup({ name: "", description: "" });
        }
        catch (err) {
            console.log(err.response.data);

            setAlertType("alert-danger");
            const regex = new RegExp('Duplicate')
            if (regex.test(err.response.data))
                setAlertMessage("Group name already in use");
            else
                setAlertMessage("Error Occurred")
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
            <h3>Register</h3>
            {alert}

            <Formik
                initialValues={{
                    name: '',
                    description: ''
                }}
                validationSchema={GroupSchema}
                onSubmit={handleSubmit}
            >
                {({ errors, touched }) => (
                    <Form>

                        <div className="row mb-3 justify-content-center">

                            <div className="col-lg-6">
                                <label htmlFor="name" className="form-label">Name</label>
                                <Field type="text" required className="form-control"
                                    id="name" aria-describedby="userHelp" name="name" />
                                {/* onChange={this.handleChange} value={this.state.username} ></input>*/}
                                {errors.name && touched.name ? (<div className="text-danger">{errors.name}</div>) : null}

                            </div>
                        </div>
                        <div className="row mb-3 justify-content-center">
                            <div className="col-lg-6">
                                <label htmlFor="description" className="form-label">description</label>
                                <Field as="textarea" type="text" name="description" id="description" required className="form-control" rows="5" />
                                {/* aria-describedby="pwHelp" onChange={this.handleChange} value={this.state.password} ></input>*/}
                                {errors.description && touched.description ? (<div className="text-danger">{errors.description}</div>) : null}
                            </div>
                        </div>
                        <div className="row mb-3">
                            <div className="col-lg-3">
                                <button className="btn btn-success" type="submit">Create Group</button>
                            </div>
                        </div>
                    </Form>
                )}
            </Formik>
        </div>
    );

}
