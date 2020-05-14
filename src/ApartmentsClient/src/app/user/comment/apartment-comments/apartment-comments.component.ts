import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CommentUserService,
  CommentDTO,
  PagedResponseOfCommentDTO
} from 'src/app/services/nswag.generated.service'
import { LoggedService } from 'src/app/services/logged.service'
import * as jwt_decode from 'jwt-decode';

@Component({
  selector: 'app-apartment-comments',
  templateUrl: './apartment-comments.component.html',
  styleUrls: ['./apartment-comments.component.scss']
})
export class ApartmentCommentsComponent implements OnInit {

  comments: CommentDTO[];
  response: PagedResponseOfCommentDTO;
  requestForm: FormGroup;
  pages: number[] = [];

  constructor(
    private commentService: CommentUserService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private authService: LoggedService
  ) { 
    this.requestForm = this.fb.group({
      data: this.route.snapshot.paramMap.get('id'),
      pageNumber: 1,
      pageSize: 20
    })
  }

  ngOnInit(): void {
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
    this.commentService.getAllCommentsByApartmentId(this.requestForm.value)
    .subscribe(response => {
      this.response = response;

      this.pages = [];
      for (let index = 1; index <= response.totalPages; index++) {
        this.pages.push(index);
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
