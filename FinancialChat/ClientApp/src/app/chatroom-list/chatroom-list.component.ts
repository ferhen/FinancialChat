// core
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ChatroomListService } from './chatroom-list.service';
import { Observable, BehaviorSubject, Subject } from 'rxjs';

// imports
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

// modals
import { DeleteChatroomModalComponent } from './delete-chatroom-modal/delete-chatroom-modal.component';
import { CreateChatroomModalComponent } from './create-chatroom-modal/create-chatroom-modal.component';

// models
import { IChatroom } from './chatroom.model';
import { map, tap, takeUntil } from 'rxjs/operators';

@Component({
    templateUrl: './chatroom-list.component.html',
    styleUrls: ['./chatroom-list.component.css']
})
export class ChatroomListComponent implements OnInit, OnDestroy {
    public chatrooms = new BehaviorSubject<IChatroom[]>([]);
    private readonly destroy$ = new Subject();

    constructor(private readonly service: ChatroomListService,
        private readonly modalService: NgbModal) { }

    public create(): void {
        this.modalService.open(CreateChatroomModalComponent).result.then(chatroomName => {
            this.service.createChatroom(chatroomName)
                .pipe(takeUntil(this.destroy$))
                .subscribe(
                    chatroom => this.chatrooms.next(
                        [...this.chatrooms.getValue(), chatroom]
                    )
                );
        }, () => { });
    }

    public edit(event: MouseEvent, chatroom: IChatroom): void {
        event.stopPropagation();
        event.preventDefault();
    }

    public remove(event: MouseEvent, chatroom: IChatroom): void {
        event.stopPropagation();
        event.preventDefault();
        this.modalService.open(DeleteChatroomModalComponent, { size: 'sm' }).result.then(() => {
            this.service.deleteChatroom(chatroom.id)
                .pipe(takeUntil(this.destroy$))
                .subscribe(
                    deletedChatroomId => this.chatrooms.next(
                        this.chatrooms.getValue().filter(x => x.id !== deletedChatroomId)
                    )
                );
        }, () => { });
    }

    private getChatrooms(): void {
        this.service.getChatrooms()
            .pipe(takeUntil(this.destroy$))
            .subscribe(chatrooms => this.chatrooms.next(chatrooms));
    }

    ngOnInit(): void {
        this.getChatrooms();
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
}
