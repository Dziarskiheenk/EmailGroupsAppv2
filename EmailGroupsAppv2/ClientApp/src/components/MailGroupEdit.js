import React, { useState } from "react";
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Form, FormGroup, Label, Input, FormFeedback } from 'reactstrap';
import axios from "axios";
import PropTypes from 'prop-types';
import authService from './api-authorization/AuthorizeService'

export default function MailGroupEdit(props) {

    const { toggleModal, onGroupEdit, editedGroup } = props;
    const adding = editedGroup === undefined;

    const [mailGroup, setMailGroup] = useState(editedGroup ? editedGroup : { name: undefined, description: undefined });

    const [invalidState, setInvalidState] = useState({ name: false, nameFeedback: '' });

    const validateForm = () => {
        const nameIsInvalid = !mailGroup.name;
        setInvalidState({ name: nameIsInvalid, nameFeedback: 'Name is required' });
        if (nameIsInvalid)
            return false;
        else
            return true;
    }

    const createMailGroup = async mailGroup => {
        debugger;
        const token = await authService.getAccessToken();
        debugger;
        axios.post(
            'api/MailGroups',
            mailGroup,
            { headers: !token ? {} : { 'Authorization': `Bearer ${token}` } })
            .then(response => {
                debugger;
                onGroupEdit(response.data);
                toggleModal();
            })
            .catch(err => {
                debugger;
                if (err.response.status === 409)
                    setInvalidState({ name: true, nameFeedback: 'Name is taken' });
                else
                    console.error(err);
            });
    }

    const editMailGroup = async mailGroup => {
        const token = await authService.getAccessToken();
        axios.put(
            'api/MailGroups/' + mailGroup.id, mailGroup,
            { headers: !token ? {} : { 'Authorization': `Bearer ${token}` } })
            .then(() => {
                onGroupEdit(mailGroup);
                toggleModal();
            })
            .catch(err => {
                if (err.response.status === 409)
                    setInvalidState({ name: true, nameFeedback: 'Name is taken' });
                else
                    console.error(err);
            });
    }

    return (
        <Modal isOpen={true}>
            <ModalHeader toggle={toggleModal}>{adding ? 'Adding ' : 'Editing '}mail group</ModalHeader>
            <ModalBody>
                <Form>
                    <FormGroup>
                        <Label for="mailGroupName">Name</Label>
                        <Input
                            type="text"
                            id="mailGroupName"
                            defaultValue={mailGroup.name}
                            onChange={e => {
                                setMailGroup({ ...mailGroup, name: e.target.value });
                            }}
                            invalid={invalidState.name} />
                        <FormFeedback>{invalidState.nameFeedback}</FormFeedback>
                    </FormGroup>
                    <FormGroup>
                        <Label for="mailGroupDescription">Description</Label>
                        <Input
                            type="textarea"
                            id="mailGroupDescription"
                            defaultValue={mailGroup.description}
                            onChange={e => {
                                setMailGroup({ ...mailGroup, description: e.target.value });
                            }} />
                    </FormGroup>
                </Form>
            </ModalBody>
            <ModalFooter>
                <Button color={'secondary'} onClick={toggleModal}>Cancel</Button>
                <Button
                    color={'success'}
                    onClick={() => {
                        if (!validateForm()) return;
                        if (adding)
                            createMailGroup(mailGroup);
                        else
                            editMailGroup(mailGroup);
                    }}>
                    Save
                </Button>
            </ModalFooter>
        </Modal>
    )
}

MailGroupEdit.propTypes = {
    toggleModal: PropTypes.func.isRequired,
    onGroupEdit: PropTypes.func.isRequired,
    editedGroup: PropTypes.object
}