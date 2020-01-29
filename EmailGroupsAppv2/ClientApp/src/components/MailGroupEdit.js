import React, { useState } from "react";
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Form, FormGroup, Label, Input, FormFeedback } from 'reactstrap';
import axios from "axios";
import PropTypes from 'prop-types';

export default function MailGroupEdit(props) {

    const { toggleModal, onGroupEdit, editedGroup } = props;
    const adding = editedGroup === undefined;

    const [mailGroup, setMailGroup] = useState(editedGroup ? editedGroup : { name: undefined, description: undefined });

    const [invalidState, setInvalidState] = useState({ name: false, nameFeedback: '' });

    const validateForm = () => {
        const nameIsValid = mailGroup.name != undefined;
        setInvalidState({ name: !nameIsValid, nameFeedback: 'Name is required' });
        if (!nameIsValid)
            return false;
        else
            return true;
    }

    const createMailGroup = mailGroup => {
        axios.post('api/MailGroups', mailGroup)
            .then(response => {
                onGroupEdit(response.data);
                toggleModal();
            })
            .catch(err => {
                if (err.response.status == 409)
                    setInvalidState({ name: true, nameFeedback: 'Name is taken' });
                else
                    console.error(err);
            });
    }

    const editMailGroup = mailGroup => {
        axios.put('api/MailGroups/' + mailGroup.id, mailGroup)
            .then(() => {
                onGroupEdit(mailGroup);
                toggleModal();
            })
            .catch(err => {
                if (err.response.status == 409)
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