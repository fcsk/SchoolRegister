import React, { Component } from 'react';
import './Auth.css';
import AuthService from './AuthService';

class Register extends Component {
    constructor() {
        super();
        this.handleChange = this.handleChange.bind(this);
        this.handleFormSubmit = this.handleFormSubmit.bind(this);
        this.Auth = new AuthService();
    }
    state = { initDone: false }
    componentWillMount() {
        if (this.Auth.loggedIn())
            this.props.history.replace('/');
    }
    componentDidMount() {
    }

    handleChange(e) {
        this.setState(
            {
                [e.target.name]: e.target.value
            }
        );
    }

    handleFormSubmit(e) {
        e.preventDefault();
        const localThis = this;
        if (this.state.username !== "" && this.state.password !== "" && this.state.confirmPassword !== "") {
            if (this.state.password === this.state.confirmPassword) {
                this.Auth.register(this.state.username, this.state.password, this.state.confirmPassword)
                    .then((result) => {
                        if (localThis.Auth.isSuccess === true) {
                            alert("You are registered in the system.");
                            this.props.history.replace('/login');
                        }
                    })
                    .catch(err => {
                        alert(err);
                    });
            } else {
                alert("Password and confirm password are not the same.");
            }
        } else {
            alert("Provide all data.");
        }
    }

    render() {
        return (
            <div className="center">
                <div className="card">
                    <h1>{"Register"}</h1>
                    <form onSubmit={this.handleFormSubmit}>
                        <input
                            className="form-item"
                            placeholder={"Username"}
                            name="username"
                            type="text"
                            onChange={this.handleChange}
                        />
                        <input
                            className="form-item"
                            placeholder={"password"}
                            name="password"
                            type="password"
                            onChange={this.handleChange}
                        />
                        <input
                            className="form-item"
                            placeholder={"confirm password"}
                            name="confirmPassword"
                            type="password"
                            onChange={this.handleChange}
                        />
                        <input
                            className="form-submit"
                            value={"Register"}
                            type="submit"
                        />
                        <label className="" />
                    </form>
                </div>
            </div>
        );
    }
}

export default Register;