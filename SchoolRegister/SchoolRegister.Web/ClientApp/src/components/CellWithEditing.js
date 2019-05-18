/* global location */
/* eslint no-restricted-globals: ["off", "confirm"] */
import React from 'react';
import { GridCell } from '@progress/kendo-react-grid';

export default function CellWithEditing(edit, remove) {
    return class extends GridCell {
        render() {
            return (
                <td>
                    {edit !== null ?
                        <button
                            className="k-primary k-button k-grid-edit-command"
                            onClick={() => { edit(this.props.dataItem); }}
                        >
                            Edit
                    </button> : ""}
                    &nbsp;
                    <button
                        className="k-button k-grid-remove-command"
                        onClick={() => {
                            confirm('Confirm deleting') &&
                                remove(this.props.dataItem);
                        }}
                    >
                        Delete
                    </button>
                </td>
            );
        }
    };
}