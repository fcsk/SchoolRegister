import React, { Component } from 'react';
import './Auth.css';
import AuthService from './AuthService';
import { Link } from 'react-router-dom';

class Login extends Component {
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
        if (this.state.username !== "" && this.state.password !== "") {
            this.Auth.login(this.state.username, this.state.password)
                .then(() => {
                    if (localThis.Auth.isSuccess === true) {
                        this.props.history.replace('/');
                    }
                })
                .catch(err => {
                    alert(err);
                });
        }
    }

    render() {
        return (
            <div className="center">
                <div className="card">
                    <h1>{"Login"}</h1>
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
                            className="form-submit"
                            value={"Login"}
                            type="submit"
                        />
                        <label className="" />
                    </form>
                    <Link to={'/register'}>Register</Link>
                </div>
            </div>
        );
    }
}
export default Login;