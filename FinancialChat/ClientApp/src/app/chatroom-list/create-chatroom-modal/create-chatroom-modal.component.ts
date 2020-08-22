import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    templateUrl: './create-chatroom-modal.component.html'
})
export class CreateChatroomModalComponent {
    public chatroomName = '';

    constructor(public readonly activeModal: NgbActiveModal) { }
}
