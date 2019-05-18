import React, { Component } from 'react';
import { Grid, GridColumn as Column, GridToolbar } from '@progress/kendo-react-grid';
import { Input } from '@progress/kendo-react-inputs';
import AuthService from '../components/Authentication/AuthService';
import Dialog from './Dialog';
import cellWithEditing from './CellWithEditing';
import { filterBy } from '@progress/kendo-data-query';
import { orderBy } from '@progress/kendo-data-query';
import { DropDownList } from '@progress/kendo-react-dropdowns';
const authService = new AuthService();

export class Subject extends Component {

    grid;
    displayName = Subject.name

    constructor(props) {
        super(props);
        this.state = {
            data: [],
            dataInEdit: undefined,
            filter: {
                logic: "",
                filters: []
            },
            sort: [],
            teachers: []
        };
        this.edit = this.edit.bind(this);
        this.save = this.save.bind(this);
        this.remove = this.remove.bind(this);
        this.cancel = this.cancel.bind(this);
        this.insert = this.insert.bind(this);
        this.onDialogInputChange = this.onDialogInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleTeacherDropdownChange = this.handleTeacherDropdownChange.bind(this);
    }

    componentDidMount() {
        if (authService.loggedIn()) {
            authService.fetch(`${authService.domain}/Subject/GetSubjects`,
                {
                    method: 'GET'
                },
                authService.getToken()).then(res => {
                    this.setState({ data: res });
                });

            authService.fetch(`${authService.domain}/Subject/GetTeachers`,
                {
                    method: 'GET'
                },
                authService.getToken()).then(res => {
                    this.setState({
                        teachers: res,
                        defaultTeacher: res.slice()[0]
                    });
                });
        }
    }

    handleSubmit(event) {
        event.preventDefault();
    }

    edit(dataItem) {
        this.setState({ dataInEdit: this.cloneItem(dataItem) });
    }

    remove(dataItem) {
        if (authService.loggedIn()) {
            const items = this.state.data.slice();
            const index = items.findIndex(p => p.id === dataItem.id);
            if (index !== -1) {
                var formData = new FormData();
                formData.append("id", dataItem.id);
                authService.fetch(`${authService.domain}/Subject/DeleteSubject`,
                    {
                        body: formData,
                        method: 'DELETE'
                    },
                    authService.getToken()).then(() => {
                        authService.fetch(`${authService.domain}/Subject/GetSubjects`,
                            {
                                method: 'GET'
                            }, authService.getToken()).then(res => {
                                this.setState({ data: res });
                            });
                    });
            }
        }
    }

    save() {
        if (authService.loggedIn()) {
            const dataItem = this.state.dataInEdit;
            if ((dataItem.name !== undefined && dataItem.name !== "") &&
                (dataItem.description !== undefined && dataItem.description !== "") &&
                (dataItem.teacher !== undefined && dataItem.teacher.id !== 0)) {
                const data = this.state.data.slice();
                if (dataItem.id !== undefined) {
                    const index = data.findIndex(p => p.id === dataItem.id);
                    data.splice(index, 1, dataItem);
                }
                var formData = new FormData();
                formData.append("name", dataItem.name);
                formData.append("description", dataItem.description);
                formData.append("teacherId", dataItem.teacher.id);
                formData.append("id", dataItem.id === undefined ? "" : dataItem.id);
                authService.fetch(`${authService.domain}/Subject/AddOrUpdateSubject`,
                    {
                        body: formData,
                        method: 'POST'
                    },
                    authService.getToken()).then(() => {
                        authService.fetch(`${authService.domain}/Subject/GetSubjects`,
                            {
                                method: 'GET'
                            }, authService.getToken()).then(res => {
                                res.forEach(function (obj) {
                                    obj.creationDate = new Date(obj.creationDate);
                                });
                                this.setState({
                                    data: res,
                                    dataInEdit: undefined
                                });
                            });
                    });
            }
        }
    }

    cancel() {
        this.setState({ dataInEdit: undefined });
    }

    insert() {
        this.setState({ dataInEdit: {} });
    }

    onDialogInputChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.props ? target.props.name : target.name;
        const edited = this.cloneItem(this.state.dataInEdit);
        edited[name] = value;
        this.setState({
            dataInEdit: edited
        });
    }

    dialogTitle() {
        return this.state.dataInEdit.id === undefined ? "ADD" : "EDIT";
    }

    cloneItem(item) {
        const clonedItem = {
            id: item.id,
            name: item.name,
            description: item.description,
            teacher: item.teacherName && item.teacherId ? {
                name: item.teacherName,
                id: item.teacherId
            } : item.teacher
        };
        return Object.assign({}, clonedItem);
    }

    newItem(source) {
        const newItem = {
            name: "",
            description: "",
            teacher: {}
        };

        return Object.assign(newItem, source);
    }

    handleTeacherDropdownChange(event) {
        const edited = this.state.dataInEdit;
        edited.teacher = {
            id: event.target.value.id,
            name: event.target.value.name
        };
        this.setState({
            dataInEdit: edited
        });
    }

    render() {
        return (
            <div>
                <Grid
                    ref={(grid) => this.grid = grid}
                    data={orderBy(filterBy(this.state.data, this.state.filter), this.state.sort)}
                    style={{ height: 'auto' }}
                    filterable
                    filter={this.state.filter}
                    onFilterChange={(e) => {
                        this.setState({
                            filter: e.filter
                        });
                    }}
                    sortable
                    sort={this.state.sort}
                    onSortChange={(e) => {
                        this.setState({
                            sort: e.sort
                        });
                    }}
                >
                    <GridToolbar>
                        <button
                            onClick={this.insert}
                            className="k-button"
                        >
                            Add
                        </button>

                    </GridToolbar>
                    <Column field="name" title="name" width="150px" filter="text" />
                    <Column field="description" title="description" width="250px" />
                    <Column field="teacherName" title="teacherName" width="200px" />
                    <Column
                        title="edit/delete"
                        filterable={false}
                        cell={cellWithEditing(this.edit, this.remove)}
                    />
                </Grid>
                {this.state.dataInEdit &&
                    <Dialog
                        title={this.dialogTitle()}
                        close={this.cancel}
                        ok={this.save}
                        cancel={this.cancel}
                    >
                        <form onSubmit={this.handleSubmit}>
                            <div style={{ marginBottom: '1rem' }}>
                                <label>
                                    Name<br />
                                    <Input
                                        type="text"
                                        name="name"
                                        value={this.state.dataInEdit.name || ''}
                                        onChange={this.onDialogInputChange}
                                    />
                                </label>
                            </div>
                            <div style={{ marginBottom: '1rem' }}>
                                <label>
                                    Description <br />
                                    <Input
                                        type="text"
                                        name="description"
                                        value={this.state.dataInEdit.description || ''}
                                        onChange={this.onDialogInputChange}
                                    />
                                </label>
                            </div>
                            <div style={{ marginBottom: '1rem' }}>
                                <label>
                                    Teacher<br />
                                    <DropDownList
                                        data={this.state.teachers}
                                        textField="name"
                                        dataItemKey="id"
                                        onChange={this.handleTeacherDropdownChange}
                                        value={this.state.dataInEdit.teacher}
                                    />
                                </label>
                            </div>
                        </form>
                    </Dialog>}
            </div>
        );
    }
}