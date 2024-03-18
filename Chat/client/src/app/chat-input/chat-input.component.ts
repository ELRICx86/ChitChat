import { Component, Output, EventEmitter} from '@angular/core';


@Component({
  selector: 'app-chat-input',
  templateUrl: './chat-input.component.html',
  styleUrl: './chat-input.component.css'
})
export class ChatInputComponent {
content: string = "";
@Output() contentEmit = new EventEmitter<string>();
sendMessage() {
  if(this.content.trim()!==""){
      this.contentEmit.emit(this.content);
  }
  this.content="";
}

}
