import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { LoggedService } from 'src/app/services/logged.service'
import { CommentUserService } from 'src/app/services/nswag.generated.service'
import * as jwt_decode from 'jwt-decode';

@Component({
  selector: 'app-add-comment',
  templateUrl: './add-comment.component.html',
  styleUrls: ['./add-comment.component.scss']
})
export class AddCommentComponent implements OnInit {

  commentForm: FormGroup;

  constructor(
    private commentService: CommentUserService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private authService: LoggedService
  ) { 
    this.commentForm = this.fb.group({
      apartmentId: this.getApartmentId(),
      title: this.getNickName(),
      text: ''
    });
  }

  ngOnInit(): void {
  }

  getApartmentId(): string {
    const id = this.route.snapshot.paramMap.get('id');
    return id;
  }

  getNickName(){
    var token = this.authService.getToken();
      
    var decodedoken = jwt_decode(token);
    var currentNickName = decodedoken['name'];

    return currentNickName;
  }
 
  add(){
     this.commentService.createComment(this.commentForm.value)
     .subscribe(data =>
       this.commentForm.reset()
     );
  }
 
}
