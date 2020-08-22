import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    templateUrl: './edit-chatroom-modal.component.html'
})
export class EditChatroomModalComponent {
    @Input()
    public chatroomName = '';

    constructor(public readonly activeModal: NgbActiveModal) { }
}
