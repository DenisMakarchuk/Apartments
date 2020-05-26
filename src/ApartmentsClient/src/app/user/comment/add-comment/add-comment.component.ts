import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { LoggedService } from 'src/app/services/logged.service'
import { CommentUserService } from 'src/app/services/nswag.generated.service'
import { GetCommentsService } from 'src/app/services/getComments.service';
import * as jwt_decode from 'jwt-decode';

@Component({
  selector: 'app-add-comment',
  templateUrl: './add-comment.component.html',
  styleUrls: ['./add-comment.component.scss']
})
export class AddCommentComponent implements OnInit {

  spinning = false;
  isAdded = false;
  errorMessage: string;

  commentForm: FormGroup;

  constructor(
    private commentService: CommentUserService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private authService: LoggedService,
    private postman: GetCommentsService
  ) { 
    this.commentForm = this.fb.group({
      apartmentId: this.getApartmentId(),
      title: this.getNickName(),
      text: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(255)]]
    });
  }

  ngOnInit(): void {
  }

  get _comment(){
    return this.commentForm.get('text');
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
    this.errorMessage = null;
    this.spinning = true;

    this.commentForm.value.apartmentId = this.getApartmentId();
    this.commentForm.value.title = this.getNickName();

    if (this.commentForm.valid) {
      this.commentService.createComment(this.commentForm.value)
      .subscribe(data =>
        {
          this.spinning = false;
          this.isAdded = true;
          this.postman.go(true);
          this.commentForm.reset();
        }
      ),
      error=>{
        this.spinning = false;
 
        if (error.status ===  500) {
          this.errorMessage = "Error 500: Internal Server Error";
        }
        if (error.status ===  400) {
          this.errorMessage = "Error 400: " + error.response;
        }
        if (error.status ===  403) {
          this.errorMessage = "Error 403: You are not authorized";
        }
        else{
          this.errorMessage = "Unsuspected Error";
        }
      };
    }    
    else
    {
      this.spinning = false;
      this.errorMessage = "Invalid data entry";
    }
  }
}
