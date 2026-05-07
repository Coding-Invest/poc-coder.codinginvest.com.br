import { Component, OnInit, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';

@Component({
  selector: 'app-chatbot',
  templateUrl: './chatbot.component.html',
  styleUrls: ['./chatbot.component.css'],
  standalone: false
})
export class ChatbotComponent implements OnInit, AfterViewChecked  {
  messages: string = '';
  chatHistory: { sender: string, text: string }[] = [];
  isMinimized: boolean = true;
  isTyping: boolean = false;
  shouldScroll: boolean = false;

  @ViewChild('chatBody') chatBodyRef!: ElementRef;
  @ViewChild('messageInput') messageInputRef!: ElementRef;

  ngOnInit(): void {
  }

  toggleChat(): void {
    this.isMinimized = !this.isMinimized;
  }

    ngAfterViewChecked(): void {
    if (this.shouldScroll) {
      setTimeout(() => {
      const el = this.chatBodyRef.nativeElement;
      el.scrollTop = el.scrollHeight;
      }, 300);
      this.shouldScroll = false;
    }
  }

  sendMessage(): void {
    const message = this.messages.trim();
    if (!message) return;

    this.chatHistory.push({ sender: 'user', text: message });
    this.chatHistory.push({ sender: 'ai', text: '...' });
    this.messages = '';
    this.messageInputRef.nativeElement.textContent = '';
    this.isTyping = true;
    this.shouldScroll = true;

    const formData = { prompt: message };
    const token = localStorage.getItem('accessToken');
    const headers: Record<string, string> = {
      'Content-Type': 'application/json',
      'Transfer-Encoding': 'chunked',
    };
    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    fetch('/api/chatbot/send', {
      method: 'POST',
      headers: headers,
      mode: 'cors',
      body: JSON.stringify(formData)
    }).then(async response => {
      if (!response.body) {
        throw new Error('ReadableStream not supported');
      }
      const reader = response.body.getReader();
      const decoder = new TextDecoder();
      let aiMessage = '...';

      while (true) {
        const { done, value } = await reader.read();
        if (done) {
          this.isTyping = false;
          break;
        }
        const answer = JSON.parse(decoder.decode(value, { stream: true }));
        if (answer.done) {
          this.isTyping = false;
          break;
        } else {
          const text = answer.message ?? answer.choices[0].message.content;
          aiMessage = text;
          this.chatHistory[this.chatHistory.length - 1].text = aiMessage;
          this.shouldScroll = true;
        }
      }
    }).catch(error => {
      alert('Error:'+ error);
      this.isTyping = false;
    });
  }
}
