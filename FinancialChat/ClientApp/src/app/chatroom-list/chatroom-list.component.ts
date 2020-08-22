// core
import { Component, OnInit } from '@angular/core';
import { ChatroomListService } from './chatroom-list.service';
import { Observable } from 'rxjs';

// imports
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

// modals
import { DeleteChatroomModalComponent } from './delete-chatroom-modal/delete-chatroom-modal.component';

// models
import { IChatroom } from './chatroom.model';

@Component({
    templateUrl: './chatroom-list.component.html',
    styleUrls: ['./chatroom-list.component.css']
})
export class ChatroomListComponent implements OnInit {
    public chatrooms = {} as Observable<IChatroom[]>;

    constructor(private readonly service: ChatroomListService,
        private readonly modalService: NgbModal) { }

    public edit(event: MouseEvent) {
        event.stopPropagation();
        event.preventDefault();
    }

    public remove(event: MouseEvent) {
        event.stopPropagation();
        event.preventDefault();
        this.modalService.open(DeleteChatroomModalComponent, { size: 'sm' });
    }

    private getChatrooms(): void {
        this.chatrooms = this.service.getChatrooms();
    }

    ngOnInit(): void {
        this.getChatrooms();
    }
}
