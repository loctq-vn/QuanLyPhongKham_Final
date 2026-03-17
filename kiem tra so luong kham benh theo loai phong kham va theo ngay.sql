SELECT count(*)
FROM quanlyphongkham.chitietkhambenh AS ctkb
INNER JOIN quanlyphongkham.khambenh AS kb 
ON ctkb.MaKhamBenh = kb.MaKhamBenh
WHERE kb.MaLoaiPhongKham = 'LPK02'
AND kb.NgayKham = '2026-03-17';