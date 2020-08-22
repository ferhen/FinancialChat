import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    templateUrl: './delete-chatroom-modal.component.html'
})
export class DeleteChatroomModalComponent {
    constructor(public readonly activeModal: NgbActiveModal) { }
}
