import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import './NavMenu.css';
import AuthService from './Authentication/AuthService';
const Auth = new AuthService();
export class NavMenu extends Component {
    displayName = NavMenu.name
    handleLogout() {
        Auth.logout();
        this.props.history.replace('/login');
    }
    render() {
        return (
            <Navbar inverse fixedTop fluid collapseOnSelect>
                <Navbar.Header>
                    <Navbar.Brand>
                        <Link to={'/'}>School Register</Link>
                    </Navbar.Brand>
                    <Navbar.Toggle />
                </Navbar.Header>
                <Navbar.Collapse>
                    <Nav>
                        <LinkContainer to={'/'} exact>
                            <NavItem>
                                <Glyphicon glyph='glyphicon glyphicon-th' /> Fetch Data
</NavItem>
                        </LinkContainer>
                        <NavItem>
                            <div >
                                <div className="row">
                                    <div className="col-md-12">
                                        <button type="button" className="form-submit center-block" onClick={this.handleLogout.bind(this)}>Logout</button>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-12">
                                        <span className="text-center center-block"> Welcome {this.props.user.username} </span>
                                    </div>
                                </div>
                            </div>
                        </NavItem>
                    </Nav>
                </Navbar.Collapse>
            </Navbar>
        );
    }
}