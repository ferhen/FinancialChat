// core
import { Component, OnInit } from '@angular/core';
import { ChatroomListService } from './chatroom-list.service';
import { Observable } from 'rxjs';

// models
import { IChatroom } from './chatroom.model';

@Component({
    templateUrl: './chatroom-list.component.html'
})
export class ChatroomListComponent implements OnInit {
    public chatrooms = {} as Observable<IChatroom[]>;

    constructor(private readonly service: ChatroomListService) { }

    private getChatrooms(): void {
        this.chatrooms = this.service.getChatrooms();
    }

    ngOnInit(): void {
        this.getChatrooms();
    }
}
