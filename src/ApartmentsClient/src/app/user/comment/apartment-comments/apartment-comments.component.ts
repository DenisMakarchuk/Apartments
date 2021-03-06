import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CommentUserService,
  CommentDTO,
  PagedResponseOfCommentDTO
} from 'src/app/services/nswag.generated.service';
import { GetCommentsService } from 'src/app/services/getComments.service';
import { LoggedService } from 'src/app/services/logged.service';
import * as jwt_decode from 'jwt-decode';

@Component({
  selector: 'app-apartment-comments',
  templateUrl: './apartment-comments.component.html',
  styleUrls: ['./apartment-comments.component.scss']
})
export class ApartmentCommentsComponent implements OnInit {

  spinning = false;
  errorMessage: string;

  comments: CommentDTO[];
  response: PagedResponseOfCommentDTO;
  requestForm: FormGroup;
  pages: number[] = [];

  constructor(
    private commentService: CommentUserService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private authService: LoggedService,
    private postman: GetCommentsService
  ) { 
    this.requestForm = this.fb.group({
      data: this.route.snapshot.paramMap.get('id'),
      pageNumber: 1,
      pageSize: 20
    });

    this.postman.go$
    .subscribe(go => {
      if (go) {
        this.getComments();
      }
    });
  }

  ngOnInit(): void {
  }

  get isAdmin(){
    var token = this.authService.getToken();
      
    var decodedoken = jwt_decode(token);
    var currentRole = decodedoken['role'];

    if (currentRole.includes('Admin')) {
      return true;
    }
    return false;
  }

  isAuthor(comment: CommentDTO){
    var token = this.authService.getToken();
      
    var decodedoken = jwt_decode(token);
    var currentUserId = decodedoken['id'];

    if (currentUserId === comment.authorId) {
      return true;
    }
    return false;
  }

  getComments(){
    this.errorMessage = null;
    this.spinning = true;
    
    this.commentService.getAllCommentsByApartmentId(this.requestForm.value)
    .subscribe(response => {

      this.spinning = false;
      this.response = response;

      this.pages = [];
      for (let index = 1; index <= response.totalPages; index++) {
        this.pages.push(index);
      }
    },
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
        this.errorMessage = "An error occurred.";
      }
    })
  }

  previousePage(){
    if (this.response != null && this.response.hasPreviousPage) {
      this.requestForm.value.pageNumber -= 1;
      this.getComments();
    }
  }

  nextPage(){
    if (this.response != null && this.response.hasNextPage) {
      this.requestForm.value.pageNumber += 1;
      this.getComments();
    }
  }

  getPage(page: number){
    if (this.response != null && page > 0 && page <= this.response.totalPages) {
      this.requestForm.value.pageNumber = page;
      this.getComments();
    }
  }

}
