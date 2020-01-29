import React, { useState } from "react";
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Form, FormGroup, Label, Input, FormFeedback } from 'reactstrap';
import axios from "axios";

export default function MailAddressEdit(props) {
    const { editedMailAddress, toggleModal, mailGroup } = props;
    const adding = editedMailAddress === undefined;
    const initialState = editedMailAddress ? editedMailAddress : { name: undefined, lastName: undefined, address: undefined, groupId: mailGroup.id };
    const [mailAddress, setMailAddress] = useState(initialState);

    const [invalidState, setInvalidState] = useState({ address: false, name: false, lastName: false });
    const validateForm = () => {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        const addressIsValid = re.test(String(mailAddress.address).toLowerCase());
        const nameIsValid = mailAddress.name != undefined;
        const lastNameIsValid = mailAddress.lastName != undefined;
        setInvalidState({
            address: !addressIsValid,
            name: !nameIsValid,
            lastName: !lastNameIsValid
        });
        if (!addressIsValid || !nameIsValid || !lastNameIsValid)
            return false;
        else
            return true;
    }

    const createMailAddress = () => {
        axios.post('api/MailGroups/' + mailGroup.id + '/MailAddresses', mailAddress)
            .then(response => {
                mailGroup.addresses.push(response.data)
                toggleModal();
            });
    }

    const updateMailAddress = () => {
        axios.put('api/MailGroups/' + mailGroup.id + '/MailAddresses/' + mailAddress.id, mailAddress)
            .then(() => {
                const index = mailGroup.addresses.findIndex(x => x.id == mailAddress.id);
                mailGroup.addresses = [
                    ...mailGroup.addresses.slice(0, index),
                    mailAddress,
                    ...mailGroup.addresses.slice(index + 1, mailGroup.addresses.length)
                ];
                toggleModal();
            });
    }

    return (
        <Modal isOpen={true}>
            <ModalHeader toggle={toggleModal}>{adding ? 'Adding ' : 'Editing '}mail address</ModalHeader>
            <ModalBody>
                <Form>
                    <FormGroup>
                        <Label for="mailAddress">E-mail</Label>
                        <Input
                            type="email"
                            id="mailAddress"
                            defaultValue={mailAddress.address}
                            onChange={e => {
                                setMailAddress({ ...mailAddress, address: e.target.value });
                            }}
                            invalid={invalidState.address}
                        />
                        <FormFeedback>It is not a valid e-mail address</FormFeedback>
                    </FormGroup>
                    <FormGroup>
                        <Label for="mailAddressName">Name</Label>
                        <Input
                            type="text"
                            id="mailAddressName"
                            defaultValue={mailAddress.name}
                            onChange={e => {
                                setMailAddress({ ...mailAddress, name: e.target.value });
                            }}
                            invalid={invalidState.name}
                        />
                        <FormFeedback>Name is required</FormFeedback>
                    </FormGroup>
                    <FormGroup>
                        <Label for="mailAddressLastName">Last name</Label>
                        <Input
                            type="text"
                            id="mailAddressLastName"
                            defaultValue={mailAddress.lastName}
                            onChange={e => {
                                setMailAddress({ ...mailAddress, lastName: e.target.value })
                            }}
                            invalid={invalidState.lastName}
                        />
                        <FormFeedback>Last name is required</FormFeedback>
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
                            createMailAddress();
                        else
                            updateMailAddress();
                    }}>
                    Save
                    </Button>
            </ModalFooter>
        </Modal>
    )
}