// core
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';

// services
import { ChatroomService } from './chatroom.service';

// models
import { IChatroom } from '../chatroom-list/chatroom.model';

@Component({
    templateUrl: './chatroom.component.html',
    styleUrls: ['./chatroom.component.css']
})
export class ChatroomComponent implements OnInit {
    public chatroom: Observable<IChatroom>;

    constructor(private readonly service: ChatroomService,
        private readonly route: ActivatedRoute) { }

    private getChatroomInfo(id: number) {
        this.chatroom = this.service.getChatroomInfo(id);
    }

    private getMessages(): void {

    }

    ngOnInit(): void {
        const { id } = this.route.snapshot.params;
        this.getChatroomInfo(id);
        this.getMessages();
    }
}
